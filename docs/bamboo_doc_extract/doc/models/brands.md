
# Brands

## Structure

`Brands`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `PageIndex` | `int` | Required | - |
| `PageSize` | `int` | Required | - |
| `Count` | `int` | Required | - |
| `Items` | [`List<Item2>`](../../doc/models/item-2.md) | Required | - |

## Example (as JSON)

```json
{
  "pageIndex": 0,
  "pageSize": 10,
  "count": 2,
  "items": [
    {
      "id": 2118,
      "name": "Test Brand UAE",
      "sku": "TBRAND-AE",
      "countryCode": "AE",
      "currencyCode": "AED",
      "description": "test description for Brand UAE",
      "disclaimer": "disclaimer4",
      "redemptionInstructions": "Test redemption instructions for Brand UAE",
      "terms": "Test Terms and conditions for Brand UAE",
      "status": "Live",
      "logoUrl": "https://bamboo-assets.s3.amazonaws.com/app-images/brand-images/2118/card-picture",
      "modifiedDate": "2024-07-03T06:27:00.1331765",
      "faceValues": [
        {
          "min": 1,
          "max": 1
        },
        {
          "min": 10,
          "max": 10
        },
        {
          "min": 20,
          "max": 20
        }
      ],
      "categories": [
        6
      ]
    },
    {
      "id": 6132,
      "name": "Test Brand Bahrain",
      "sku": "TBRAND1-AE",
      "countryCode": "BH",
      "currencyCode": "BHD",
      "description": "Test description",
      "disclaimer": "disclaimer4",
      "redemptionInstructions": "Test redemption instructions",
      "terms": "Test terms and conditions ",
      "status": "TemporaryInactive",
      "logoUrl": "",
      "modifiedDate": "2024-10-16T16:29:48.3522299",
      "faceValues": [],
      "categories": []
    }
  ]
}
```

