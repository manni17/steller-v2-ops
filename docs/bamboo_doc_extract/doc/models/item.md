
# Item

## Structure

`Item`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `BrandCode` | `string` | Required | - |
| `ProductId` | `int` | Required | - |
| `ProductFaceValue` | `int` | Required | - |
| `Quantity` | `int` | Required | - |
| `PictureUrl` | `string` | Required | - |
| `CountryCode` | `string` | Required | - |
| `CurrencyCode` | `string` | Required | - |
| `Cards` | [`List<Card>`](../../doc/models/card.md) | Required | - |

## Example (as JSON)

```json
{
  "brandCode": "brandCode4",
  "productId": 439685,
  "productFaceValue": 10,
  "quantity": 2,
  "pictureUrl": "pictureUrl6",
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
```

