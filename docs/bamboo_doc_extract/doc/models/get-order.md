
# Get Order

## Structure

`GetOrder`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `OrderId` | `int` | Required | - |
| `RequestId` | `string` | Required | - |
| `Items` | [`List<Item>`](../../doc/models/item.md) | Required | - |
| `Status` | `string` | Required | - |
| `CreatedDate` | `string` | Required | - |
| `Total` | `double` | Required | - |
| `ErrorMessage` | `string` | Required | - |
| `OrderType` | `string` | Required | - |
| `Currency` | `string` | Required | - |

## Example (as JSON)

```json
{
  "orderId": 6779294,
  "requestId": "71ac2817-e51a-438a-bb7b-5ffdda23603c",
  "items": [
    {
      "brandCode": "brandCode0",
      "productId": 439685,
      "productFaceValue": 10,
      "quantity": 2,
      "pictureUrl": "pictureUrl8",
      "countryCode": "BH",
      "currencyCode": "USD",
      "cards": [
        {
          "id": 24153984,
          "serialNumber": "4005c08f-1aeb-4527-8eb0-ccd1c745f467",
          "cardCode": "cb05747ed9fe497b92c80f10cf8406d7",
          "pin": "392",
          "expirationDate": "2025-05-16T04:36:35.3922677",
          "status": "Sold"
        },
        {
          "id": 24153985,
          "serialNumber": "29744102-1e84-48e0-a440-8b3a5c04b4d2",
          "cardCode": "7dffe979e54c49f692e7894eb91407d1",
          "pin": "392",
          "expirationDate": "2025-05-16T04:36:35.3922628",
          "status": "Sold"
        }
      ]
    }
  ],
  "status": "Succeeded",
  "createdDate": "2024-05-16T04:36:35.3673422",
  "total": 18.9401,
  "errorMessage": "errorMessage8",
  "orderType": "Sandbox",
  "currency": "USD"
}
```

