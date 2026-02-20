
# Get Catalog

## Structure

`GetCatalog`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Brands` | [`List<Brand>`](../../doc/models/brand.md) | Required | - |

## Example (as JSON)

```json
{
  "brands": [
    {
      "internalId": "internalId8",
      "name": "name4",
      "countryCode": "countryCode0",
      "currencyCode": "currencyCode4",
      "description": "description4",
      "disclaimer": "disclaimer8",
      "redemptionInstructions": "redemptionInstructions8",
      "terms": "terms2",
      "logoUrl": "logoUrl4",
      "modifiedDate": "modifiedDate4",
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
        },
        {
          "id": 252,
          "name": "name8",
          "description": "description2"
        }
      ]
    }
  ]
}
```

