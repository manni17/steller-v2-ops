
# Price

## Structure

`Price`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Min` | [`PriceMin`](../../doc/models/containers/price-min.md) | Required | This is a container for one-of cases. |
| `Max` | [`PriceMax`](../../doc/models/containers/price-max.md) | Required | This is a container for one-of cases. |
| `CurrencyCode` | `string` | Required | - |

## Example (as JSON)

```json
{
  "min": 34.783478,
  "max": 34.783478,
  "currencyCode": "AED"
}
```

