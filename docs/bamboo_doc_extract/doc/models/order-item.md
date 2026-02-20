
# Order Item

## Structure

`OrderItem`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `CreatedDate` | `string` | Required | - |
| `ProductName` | `string` | Required | - |
| `BrandName` | `string` | Required | - |
| `BrandCurrencyCode` | `string` | Required | - |
| `BrandWithDenominationCurrency` | `string` | Required | - |
| `Denomination` | [`Denomination`](../../doc/models/denomination.md) | Required | - |
| `ClientDiscountType` | `string` | Required | - |
| `ClientDiscountValue` | `double` | Required | - |
| `ClientTransactionFeeType` | `string` | Required | - |
| `ClientTransactionFeeValue` | `int` | Required | - |
| `ClientPrice` | [`ClientPrice`](../../doc/models/client-price.md) | Required | - |
| `EstimatedPrice` | [`EstimatedPrice`](../../doc/models/estimated-price.md) | Required | - |
| `Quantity` | `int` | Required | - |
| `ClientTotal` | `double` | Required | - |
| `ClientExchangeRate` | [`ClientExchangeRate`](../../doc/models/client-exchange-rate.md) | Required | - |

## Example (as JSON)

```json
{
  "createdDate": "0001-01-01T00:00:00",
  "productName": "Steam 10 USD BH",
  "brandName": "Steam Bahrain",
  "brandCurrencyCode": "USD",
  "brandWithDenominationCurrency": "Steam Bahrain USD",
  "denomination": {
    "value": 10,
    "currencyCode": "USD"
  },
  "clientDiscountType": "Percentage",
  "clientDiscountValue": 5.2995,
  "clientTransactionFeeType": "Fixed",
  "clientTransactionFeeValue": 0,
  "clientPrice": {
    "value": 9.47,
    "currencyCode": "USD"
  },
  "estimatedPrice": {
    "value": 9.47,
    "currencyCode": "USD"
  },
  "quantity": 1,
  "clientTotal": 9.47,
  "clientExchangeRate": {
    "fromCurrency": "USD",
    "toCurrency": "USD",
    "exchangeRate": 0
  }
}
```

