#!/bin/bash
# Steller v2 — PostgreSQL backup
# Usage: ./backup-steller-v2-db.sh [output_dir]
# Store output off-VPS (S3, B2, etc.) for disaster recovery.
set -e
OUT_DIR="${1:-/root/backups/steller-v2}"
mkdir -p "$OUT_DIR"
STAMP=$(date +%Y%m%d_%H%M%S)
DUMP="$OUT_DIR/steller_v2_$STAMP.dump"

if ! docker ps --format '{{.Names}}' | grep -q steller-v2-postgres; then
  echo "Error: steller-v2-postgres container not running." >&2
  exit 1
fi

docker exec steller-v2-postgres pg_dump -U steller_v2_user -Fc steller_v2 > "$DUMP"
echo "Backup written: $DUMP"
# Optional: upload to S3/B2, then rm local
# aws s3 cp "$DUMP" s3://your-bucket/backups/ && rm "$DUMP"
