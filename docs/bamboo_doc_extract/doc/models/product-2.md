
# Product 2

## Structure

`Product2`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Id` | `int` | Required | - |
| `Name` | `string` | Required | - |
| `MinFaceValue` | `int` | Required | - |
| `MaxFaceValue` | `int` | Required | - |
| `Count` | [`Product2Count`](../../doc/models/containers/product-2-count.md) | Required | This is a container for one-of cases. |
| `Price` | [`Price1`](../../doc/models/price-1.md) | Required | - |
| `ModifiedDate` | `string` | Required | - |

## Example (as JSON)

```json
{
  "id": 70,
  "name": "name2",
  "minFaceValue": 224,
  "maxFaceValue": 238,
  "count": "String3",
  "price": {
    "min": 114,
    "max": 32,
    "currencyCode": "currencyCode2"
  },
  "modifiedDate": "modifiedDate2"
}
```

