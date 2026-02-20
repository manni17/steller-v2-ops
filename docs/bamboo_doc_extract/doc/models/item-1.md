
# Item 1

## Structure

`Item1`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `InternalId` | `string` | Required | - |
| `Name` | `string` | Required | - |
| `CountryCode` | `string` | Required | - |
| `CurrencyCode` | `string` | Required | - |
| `Description` | `string` | Required | - |
| `Disclaimer` | `string` | Required | - |
| `RedemptionInstructions` | `string` | Required | - |
| `Terms` | `string` | Required | - |
| `LogoUrl` | `string` | Required | - |
| `ModifiedDate` | `string` | Required | - |
| `Products` | [`List<Product2>`](../../doc/models/product-2.md) | Required | - |
| `Categories` | [`List<Item1Categories>`](../../doc/models/containers/item-1-categories.md) | Required | This is List of a container for one-of cases. |

## Example (as JSON)

```json
{
  "internalId": "internalId8",
  "name": "name4",
  "countryCode": "countryCode0",
  "currencyCode": "currencyCode4",
  "description": "description4",
  "disclaimer": "disclaimer2",
  "redemptionInstructions": "redemptionInstructions8",
  "terms": "terms2",
  "logoUrl": "logoUrl4",
  "modifiedDate": "modifiedDate4",
  "products": [
    {
      "id": 124,
      "name": "name2",
      "minFaceValue": 234,
      "maxFaceValue": 220,
      "count": "String3",
      "price": {
        "min": 114,
        "max": 32,
        "currencyCode": "currencyCode2"
      },
      "modifiedDate": "modifiedDate8"
    }
  ],
  "categories": [
    {
      "id": 252,
      "name": "name8",
      "description": "description2"
    }
  ]
}
```

