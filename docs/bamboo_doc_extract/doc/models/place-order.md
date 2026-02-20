
# Place Order

## Structure

`PlaceOrder`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `RequestId` | `string` | Required | - |
| `OrderNumber` | `string` | Required | - |
| `Status` | `string` | Required | - |
| `CurrencyCode` | `string` | Required | - |
| `TotalCards` | `int` | Required | - |

## Example (as JSON)

```json
{
  "requestId": "efb96235-5ec7-4969-852b-ccea48c8d906",
  "orderNumber": "O-10531861",
  "status": "Created",
  "currencyCode": "USD",
  "totalCards": 1
}
```

