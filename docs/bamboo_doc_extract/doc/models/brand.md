
# Brand

## Structure

`Brand`

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
| `Products` | [`List<Product>`](../../doc/models/product.md) | Required | - |
| `Categories` | [`List<BrandCategories>`](../../doc/models/containers/brand-categories.md) | Required | This is List of a container for one-of cases. |

## Example (as JSON)

```json
{
  "internalId": "internalId2",
  "name": "name8",
  "countryCode": "countryCode6",
  "currencyCode": "currencyCode8",
  "description": "description2",
  "disclaimer": "disclaimer4",
  "redemptionInstructions": "redemptionInstructions4",
  "terms": "terms6",
  "logoUrl": "logoUrl8",
  "modifiedDate": "modifiedDate8",
  "products": [
    {
      "id": 124,
      "name": "name2",
      "minFaceValue": 156,
      "maxFaceValue": 114,
      "count": "String3",
      "price": {
        "min": 34.783478,
        "max": 34.783478,
        "currencyCode": "AED"
      },
      "modifiedDate": "modifiedDate8"
    }
  ],
  "categories": [
    {
      "id": 252,
      "name": "name8",
      "description": "description2"
    },
    {
      "id": 252,
      "name": "name8",
      "description": "description2"
    }
  ]
}
```

