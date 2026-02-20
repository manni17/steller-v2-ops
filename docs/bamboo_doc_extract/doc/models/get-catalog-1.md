
# Get Catalog 1

## Structure

`GetCatalog1`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `PageIndex` | `int` | Required | - |
| `PageSize` | `int` | Required | - |
| `Count` | `int` | Required | - |
| `Items` | [`List<Item1>`](../../doc/models/item-1.md) | Required | - |

## Example (as JSON)

```json
{
  "pageIndex": 98,
  "pageSize": 70,
  "count": 128,
  "items": [
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

