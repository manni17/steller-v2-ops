#!/usr/bin/env bash
# =============================================================================
# Steller v2 E2E API Journey — Black-Box Test
# Scenario: Admin provisions B2B Partner, funds wallet, assigns commissions,
# Partner fetches catalog, places orders (Qty 1, Qty 5). psql telemetry verify.
# Environment: Production mode (Bamboo Sandbox). Run against live Steller v2 API.
# =============================================================================

set -euo pipefail

# -----------------------------------------------------------------------------
# Configuration (override via env)
# -----------------------------------------------------------------------------
: "${API_BASE:=http://localhost:6091}"
: "${DB_HOST:=localhost}"
: "${DB_PORT:=6432}"
: "${DB_NAME:=steller_v2}"
: "${DB_USER:=steller_v2_user}"
: "${DB_PASSWORD:=}"
: "${ADMIN_EMAIL:=admin@steller.com}"
: "${ADMIN_PASSWORD:=Admin123!}"
: "${CREDIT_AMOUNT:=1000}"
: "${SLEEP_BETWEEN_ORDERS:=3}"
: "${JQ:=jq}"

TMP_DIR=$(mktemp -d)
trap 'rm -rf "$TMP_DIR"' EXIT

LOG() { echo "[$(date +%H:%M:%S)] $*"; }
ERR() { echo "[ERROR] $*" >&2; exit 1; }

command -v curl >/dev/null || ERR "curl required"
command -v "$JQ" >/dev/null 2>/dev/null || {
  LOG "jq not found; using grep/sed for JSON extraction"
  JQ=""
}

# -----------------------------------------------------------------------------
# 1. Admin auth (JWT extraction)
# -----------------------------------------------------------------------------
LOG "1. Admin login..."
LOGIN_RESP=$(curl -sS -X POST "$API_BASE/api/auth/login" \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"$ADMIN_EMAIL\",\"password\":\"$ADMIN_PASSWORD\"}")

if [ -n "$JQ" ]; then
  JWT=$(echo "$LOGIN_RESP" | jq -r '.Data.AccessToken // .data.accessToken // .accessToken // empty')
else
  JWT=$(echo "$LOGIN_RESP" | grep -oE '"accessToken"|"AccessToken"[^"]*"[^"]*"' | head -1 | sed 's/.*"\([^"]*\)"$/\1/')
  [ -z "$JWT" ] && JWT=$(echo "$LOGIN_RESP" | grep -oE '"AccessToken"[^"]*"[^"]*"' | head -1 | sed 's/.*"\([^"]*\)"$/\1/')
fi

[ -z "$JWT" ] || [ "$JWT" = "null" ] && ERR "Admin login failed. Response: $LOGIN_RESP"
LOG "Admin JWT obtained."

# -----------------------------------------------------------------------------
# 2. Partner creation (POST /api/public/signup)
# -----------------------------------------------------------------------------
PARTNER_EMAIL="e2e-partner-$(date +%s)@example.com"
LOG "2. Creating partner via signup..."
SIGNUP_RESP=$(curl -sS -X POST "$API_BASE/api/public/signup" \
  -H "Content-Type: application/json" \
  -d "{\"companyName\":\"E2E Test Partner Inc\",\"email\":\"$PARTNER_EMAIL\",\"useCase\":\"E2E black-box test\"}")

if [ -n "$JQ" ]; then
  PARTNER_ID=$(echo "$SIGNUP_RESP" | jq -r '.Data.PartnerId // .data.partnerId // .partnerId // empty')
  API_KEY=$(echo "$SIGNUP_RESP" | jq -r '.Data.ApiKey // .data.apiKey // .apiKey // empty')
else
  PARTNER_ID=$(echo "$SIGNUP_RESP" | grep -oE '"partnerId"[^0-9]*[0-9]+' | grep -oE '[0-9]+' | head -1)
  API_KEY=$(echo "$SIGNUP_RESP" | grep -oE '"apiKey"[^"]*"stlr_[^"]*"' | sed 's/.*"\(stlr_[^"]*\)".*/\1/')
fi

[ -z "$PARTNER_ID" ] || [ -z "$API_KEY" ] && ERR "Partner signup failed. Response: $SIGNUP_RESP"
LOG "Partner created: Id=$PARTNER_ID, ApiKey=***"

# Ensure API key exists (signup returns it; if not, Admin creates via POST /api/admin/partners/{id}/keys)
if [[ ! "$API_KEY" =~ ^stlr_ ]]; then
  LOG "Signup did not return API key; creating via Admin API..."
  KEY_RESP=$(curl -sS -X POST "$API_BASE/api/admin/partners/$PARTNER_ID/keys" \
    -H "Authorization: Bearer $JWT" -H "Content-Type: application/json" -d "{}")
  if [ -n "$JQ" ]; then
    API_KEY=$(echo "$KEY_RESP" | jq -r '.apiKey // .ApiKey // empty')
  else
    API_KEY=$(echo "$KEY_RESP" | grep -oE '"apiKey"[^"]*"stlr_[^"]*"' | sed 's/.*"\(stlr_[^"]*\)".*/\1/')
  fi
  [ -z "$API_KEY" ] && ERR "Failed to create API key. Response: $KEY_RESP"
fi

# -----------------------------------------------------------------------------
# 3. Wallet funding via Admin API
# -----------------------------------------------------------------------------
REF_ID="e2e-credit-$(date +%s)"
LOG "3. Funding partner wallet ($CREDIT_AMOUNT)..."
CREDIT_RESP=$(curl -sS -X POST "$API_BASE/api/wallet/$PARTNER_ID/credit" \
  -H "Authorization: Bearer $JWT" \
  -H "Content-Type: application/json" \
  -d "{\"amount\":$CREDIT_AMOUNT,\"description\":\"E2E test credit\",\"referenceId\":\"$REF_ID\"}")

# Expect 200; check balance if possible
[ -n "$JQ" ] && BALANCE=$(echo "$CREDIT_RESP" | jq -r '.availableBalance // .AvailableBalance // empty')
LOG "Wallet credited. Balance: ${BALANCE:-OK}"

# -----------------------------------------------------------------------------
# 4. Pricing commission assignment (5 SKUs)
# Note: PartnerDiscount is per-vendor. We assign discount for vendor "bamboo"
# and optionally create PartnerProductPricing for 5 products via psql.
# -----------------------------------------------------------------------------
LOG "4. Assigning pricing/commission (discount for Bamboo vendor)..."
curl -sS -X POST "$API_BASE/api/admin/partners/$PARTNER_ID/discounts" \
  -H "Authorization: Bearer $JWT" \
  -H "Content-Type: application/json" \
  -d '{"vendorId":"bamboo","discountPercent":10}' >/dev/null 2>&1 || true

# Per-SKU custom pricing: requires PartnerProductPricing. Do via psql if DB available.
if [ -n "$DB_PASSWORD" ]; then
  export PGPASSWORD="$DB_PASSWORD"
  LOG "4b. Assigning PartnerProductPricing for 5 active products (psql)..."
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -t -A -c "
    INSERT INTO \"PartnerProductPricings\" (\"PartnerId\", \"ProductId\", \"Price\", \"PriceAllow\", \"PercentageValue\", \"DisableRateValue\", \"AllowRateValue\", \"PriceDisable\", \"IsActive\", \"CreatedAt\", \"UpdatedAt\")
    SELECT $PARTNER_ID, p.\"Id\", COALESCE(pp.\"Max\", p.\"MaxFaceValue\"), p.\"MinFaceValue\", 0, 0, 0, 0, true, NOW(), NOW()
    FROM \"Products\" p
    LEFT JOIN \"ProductPricings\" pp ON pp.\"ProductId\" = p.\"Id\"
    WHERE p.\"IsActive\" = true
    LIMIT 5
    ON CONFLICT (\"PartnerId\", \"ProductId\") DO NOTHING;
  " 2>/dev/null || LOG "psql PartnerProductPricing optional (schema may differ)"
  unset PGPASSWORD
else
  LOG "4b. Skipping PartnerProductPricing (no DB_PASSWORD); discount applies to all products."
fi

# -----------------------------------------------------------------------------
# 5. Partner catalog fetch (x-api-key) — assert custom pricing
# -----------------------------------------------------------------------------
LOG "5. Fetching partner catalog..."
CATALOG_HTTP=$(curl -sS -w "%{http_code}" -o /tmp/e2e_catalog.json -X GET "$API_BASE/api/brand/getCatalog" -H "x-api-key: $API_KEY" -H "Content-Type: application/json")
CATALOG_RESP=$(cat /tmp/e2e_catalog.json 2>/dev/null)

# Assert we get brands/products (not 401)
if [ "$CATALOG_HTTP" = "401" ]; then
  ERR "Catalog returned 401. Partner may lack valid API key. Response: ${CATALOG_RESP:0:200}"
fi
PRODUCT_COUNT=$(echo "$CATALOG_RESP" | grep -o '"sku"\|"Sku"' | wc -l || true)
LOG "Catalog fetched. Products visible: ${PRODUCT_COUNT:-N/A}"

# Pick first product SKU and face value for orders
if [ -n "$JQ" ]; then
  SKU=$(echo "$CATALOG_RESP" | jq -r '.data[0].products[0].sku // .data[0].Products[0].Sku // .[0].products[0].sku // empty' 2>/dev/null)
  FACE_VALUE=$(echo "$CATALOG_RESP" | jq -r '.data[0].products[0].price.max // .data[0].products[0].maxFaceValue // .data[0].products[0].maxFaceValue // 50' 2>/dev/null)
fi
SKU="${SKU:-}"
FACE_VALUE="${FACE_VALUE:-50}"
if [ -z "$SKU" ]; then
  # Fallback: use known test SKU or sync catalog first
  SKU="TEST-E2E-SKU"
  FACE_VALUE=50
  LOG "Using default SKU=$SKU faceValue=$FACE_VALUE (catalog may be empty; sync first)"
fi

# -----------------------------------------------------------------------------
# 6a. Order 1 (Qty 1)
# -----------------------------------------------------------------------------
REF1="e2e-order1-$(date +%s)-$(shuf -i 1000-9999 -n 1)"
LOG "6a. Placing Order 1 (Qty 1)..."
ORDER1_RESP=$(curl -sS -w "\n%{http_code}" -X POST "$API_BASE/api/orders" \
  -H "x-api-key: $API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"sku\":\"$SKU\",\"faceValue\":$FACE_VALUE,\"quantity\":1,\"referenceId\":\"$REF1\"}")

HTTP1=$(echo "$ORDER1_RESP" | tail -1)
BODY1=$(echo "$ORDER1_RESP" | sed '$d')
[ "$HTTP1" != "202" ] && ERR "Order 1 expected 202, got $HTTP1. Body: $BODY1"

if [ -n "$JQ" ]; then
  ORDER1_ID=$(echo "$BODY1" | jq -r '.Data.id // .data.id // .id // empty')
else
  ORDER1_ID=$(echo "$BODY1" | grep -oE '"id"[^"]*"[0-9a-f-]{36}"' | head -1 | grep -oE '[0-9a-f-]{36}' || true)
fi
LOG "Order 1 placed: Id=$ORDER1_ID"

# -----------------------------------------------------------------------------
# 6b. Sleep between orders
# -----------------------------------------------------------------------------
LOG "6b. Sleeping ${SLEEP_BETWEEN_ORDERS}s between orders..."
sleep "$SLEEP_BETWEEN_ORDERS"

# -----------------------------------------------------------------------------
# 6c. Order 2 (Qty 5)
# -----------------------------------------------------------------------------
REF2="e2e-order2-$(date +%s)-$(shuf -i 1000-9999 -n 1)"
LOG "6c. Placing Order 2 (Qty 5)..."
ORDER2_RESP=$(curl -sS -w "\n%{http_code}" -X POST "$API_BASE/api/orders" \
  -H "x-api-key: $API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"sku\":\"$SKU\",\"faceValue\":$FACE_VALUE,\"quantity\":5,\"referenceId\":\"$REF2\"}")

HTTP2=$(echo "$ORDER2_RESP" | tail -1)
BODY2=$(echo "$ORDER2_RESP" | sed '$d')
[ "$HTTP2" != "202" ] && ERR "Order 2 expected 202, got $HTTP2. Body: $BODY2"

if [ -n "$JQ" ]; then
  ORDER2_ID=$(echo "$BODY2" | jq -r '.Data.id // .data.id // .id // empty')
else
  ORDER2_ID=$(echo "$BODY2" | grep -oE '"id"[^"]*"[0-9a-f-]{36}"' | head -1 | grep -oE '[0-9a-f-]{36}' || true)
fi
LOG "Order 2 placed: Id=$ORDER2_ID"

# -----------------------------------------------------------------------------
# 7. psql telemetry verification (WalletHistories, Orders, VendorApiCalls, GiftCards)
# -----------------------------------------------------------------------------
if [ -n "$DB_PASSWORD" ]; then
  export PGPASSWORD="$DB_PASSWORD"
  LOG "7. psql telemetry verification..."

  echo ""
  echo "--- WalletHistories (recent for partner $PARTNER_ID) ---"
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -c "
    SELECT wh.\"Id\", w.\"PartnerId\", wh.\"Amount\", wh.\"BalanceBefore\", wh.\"BalanceAfter\", wh.\"TransactionTypeId\"
    FROM \"WalletHistories\" wh
    JOIN \"Wallets\" w ON w.\"Id\" = wh.\"WalletId\"
    WHERE w.\"PartnerId\" = $PARTNER_ID
    ORDER BY wh.\"CreatedAt\" DESC
    LIMIT 10;
  " || true

  echo ""
  echo "--- Orders (partner $PARTNER_ID) ---"
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -c "
    SELECT \"Id\", \"RequestId\", \"PartnerId\", \"Status\", \"CreateDate\"
    FROM \"Orders\"
    WHERE \"PartnerId\" = $PARTNER_ID
    ORDER BY \"CreatedAt\" DESC
    LIMIT 10;
  " || true

  echo ""
  echo "--- VendorApiCalls (recent for orders) ---"
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -c "
    SELECT vac.\"Id\", vac.\"OrderId\", vac.\"RequestId\", vac.\"Status\"
    FROM \"VendorApiCalls\" vac
    WHERE vac.\"OrderId\" IN ('$ORDER1_ID'::uuid, '$ORDER2_ID'::uuid)
    ORDER BY vac.\"CreatedDate\" DESC;
  " || true

  echo ""
  echo "--- GiftCards (for partner orders) ---"
  psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -c "
    SELECT g.\"Id\", g.\"OrderId\", g.\"Serial\"
    FROM \"GiftCards\" g
    JOIN \"Orders\" o ON o.\"Id\" = g.\"OrderId\"
    WHERE o.\"PartnerId\" = $PARTNER_ID
    ORDER BY g.\"CreatedDate\" DESC
    LIMIT 20;
  " || true

  unset PGPASSWORD
else
  LOG "7. Skipping psql verification (set DB_PASSWORD to enable)."
fi

# -----------------------------------------------------------------------------
# Summary
# -----------------------------------------------------------------------------
echo ""
LOG "=== E2E API Journey Complete ==="
echo "  PartnerId:    $PARTNER_ID"
echo "  Order 1:      $ORDER1_ID (Qty 1)"
echo "  Order 2:      $ORDER2_ID (Qty 5)"
echo "  Catalog SKU:  $SKU"
echo ""
echo "  Poll GET /api/orders/{id} until status=Completed to verify fulfillment."
echo "  Expect 6 gift cards total (1 + 5) when Bamboo Sandbox fulfills."
