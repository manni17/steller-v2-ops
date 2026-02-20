
# Client

## Structure

`Client`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `ClientName` | `string` | Required | - |
| `Transactions` | [`List<Transactions1>`](../../doc/models/transactions-1.md) | Required | - |

## Example (as JSON)

```json
{
  "clientName": "Bamboo Tech - Testing",
  "transactions": [
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
      "currencyCode": "currencyCode8",
      "orderDate": "2024-05-15T04:29:41.3062431",
      "transactionId": "6677709 ",
      "transactionType": "Order",
      "comment": "comment2",
      "notes": "notes8",
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
      "clientName": "clientName4",
      "orderType": "orderType6",
      "xeroCreditNoteNumber": "xeroCreditNoteNumber0"
    },
    {
      "orderId": 6779294,
      "requestId": "71ac2817-e51a-438a-bb7b-5ffdda23603c",
      "productId": 439685,
      "orderNumber": "O-06779294",
      "orderTotal": {
        "value": 18.9401,
        "currencyCode": "USD"
      },
      "orderTotalForSelectedProducts": {
        "value": 18.94,
        "currencyCode": "USD"
      },
      "currencyCode": "currencyCode8",
      "orderDate": "2024-05-16T04:36:35.4268366",
      "transactionId": "6695293 ",
      "transactionType": "Order",
      "comment": "comment2",
      "notes": "notes8",
      "availableBalance": {
        "value": 100809.5447,
        "currencyCode": "USD"
      },
      "transactionAmount": {
        "value": 18.9401,
        "currencyCode": "USD"
      },
      "transactionDate": "2024-05-16T04:36:35.4268366",
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
            "value": 18.9401,
            "currencyCode": "USD"
          },
          "estimatedPrice": {
            "value": 18.94,
            "currencyCode": "USD"
          },
          "quantity": 2,
          "clientTotal": 18.94,
          "clientExchangeRate": {
            "fromCurrency": "USD",
            "toCurrency": "USD",
            "exchangeRate": 0
          }
        }
      ],
      "clientName": "clientName4",
      "orderType": "orderType6",
      "xeroCreditNoteNumber": "xeroCreditNoteNumber0"
    }
  ]
}
```

