
# Transactions 1

## Structure

`Transactions1`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `OrderId` | `int` | Required | - |
| `RequestId` | `string` | Required | - |
| `ProductId` | `int` | Required | - |
| `OrderNumber` | `string` | Required | - |
| `OrderTotal` | [`OrderTotal`](../../doc/models/order-total.md) | Required | - |
| `OrderTotalForSelectedProducts` | [`OrderTotalForSelectedProducts`](../../doc/models/order-total-for-selected-products.md) | Required | - |
| `CurrencyCode` | `string` | Required | - |
| `OrderDate` | `string` | Required | - |
| `TransactionId` | `string` | Required | - |
| `TransactionType` | `string` | Required | - |
| `Comment` | `string` | Required | - |
| `Notes` | `string` | Required | - |
| `AvailableBalance` | [`AvailableBalance`](../../doc/models/available-balance.md) | Required | - |
| `TransactionAmount` | [`TransactionAmount`](../../doc/models/transaction-amount.md) | Required | - |
| `TransactionDate` | `string` | Required | - |
| `OrderItems` | [`List<OrderItem>`](../../doc/models/order-item.md) | Required | - |
| `ClientName` | `string` | Required | - |
| `OrderType` | `string` | Required | - |
| `XeroCreditNoteNumber` | `string` | Required | - |

## Example (as JSON)

```json
{
  "orderId": 6761537,
  "requestId": "97018cf7-98f5-40fc-a142-711d43a72e17",
  "productId": 439685,
  "orderNumber": "O-06761537",
  "orderTotal": {
    "value": 9.47,
    "currencyCode": "USD"
  },
  "orderTotalForSelectedProducts": {
    "value": 9.47,
    "currencyCode": "USD"
  },
  "currencyCode": "currencyCode4",
  "orderDate": "2024-05-15T04:29:41.3062431",
  "transactionId": "6677709 ",
  "transactionType": "Order",
  "comment": "comment8",
  "notes": "notes4",
  "availableBalance": {
    "value": 100828.4848,
    "currencyCode": "USD"
  },
  "transactionAmount": {
    "value": 9.47,
    "currencyCode": "USD"
  },
  "transactionDate": "2024-05-15T04:29:41.3062431",
  "orderItems": [
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
  ],
  "clientName": "clientName0",
  "orderType": "orderType2",
  "xeroCreditNoteNumber": "xeroCreditNoteNumber6"
}
```

