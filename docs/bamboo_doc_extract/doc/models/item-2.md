
# Item 2

## Structure

`Item2`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Id` | `int` | Required | - |
| `Name` | `string` | Required | - |
| `Sku` | `string` | Required | - |
| `CountryCode` | `string` | Required | - |
| `CurrencyCode` | `string` | Required | - |
| `Description` | `string` | Required | - |
| `Disclaimer` | `string` | Required | - |
| `RedemptionInstructions` | `string` | Required | - |
| `Terms` | `string` | Required | - |
| `Status` | `string` | Required | - |
| `LogoUrl` | `string` | Required | - |
| `ModifiedDate` | `string` | Required | - |
| `FaceValues` | [`List<Item2FaceValues>`](../../doc/models/containers/item-2-face-values.md) | Required | This is List of a container for one-of cases. |
| `Categories` | [`List<Item2Categories>`](../../doc/models/containers/item-2-categories.md) | Required | This is List of a container for one-of cases. |

## Example (as JSON)

```json
{
  "id": 2118,
  "name": "Test Brand UAE",
  "sku": "TBRAND-AE",
  "countryCode": "AE",
  "currencyCode": "AED",
  "description": "test description for Brand UAE",
  "disclaimer": "disclaimer6",
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
}
```

