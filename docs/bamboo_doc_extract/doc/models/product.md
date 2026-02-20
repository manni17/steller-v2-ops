
# Product

## Structure

`Product`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Id` | `int` | Required | - |
| `Name` | `string` | Required | - |
| `MinFaceValue` | [`ProductMinFaceValue`](../../doc/models/containers/product-min-face-value.md) | Required | This is a container for one-of cases. |
| `MaxFaceValue` | [`ProductMaxFaceValue`](../../doc/models/containers/product-max-face-value.md) | Required | This is a container for one-of cases. |
| `Count` | [`ProductCount`](../../doc/models/containers/product-count.md) | Required | This is a container for one-of cases. |
| `Price` | [`Price`](../../doc/models/price.md) | Required | - |
| `ModifiedDate` | `string` | Required | - |

## Example (as JSON)

```json
{
  "id": 180,
  "name": "name4",
  "minFaceValue": 0,
  "maxFaceValue": 14,
  "count": "String1",
  "price": {
    "min": 34.783478,
    "max": 34.783478,
    "currencyCode": "AED"
  },
  "modifiedDate": "modifiedDate4"
}
```

