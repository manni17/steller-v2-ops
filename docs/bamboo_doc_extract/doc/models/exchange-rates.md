
# Exchange Rates

## Structure

`ExchangeRates`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `BaseCurrencyCode` | `string` | Required | - |
| `Rates` | [`List<Rate>`](../../doc/models/rate.md) | Required | - |

## Example (as JSON)

```json
{
  "baseCurrencyCode": "AED",
  "rates": [
    {
      "currencyCode": "INR",
      "value": 22.718078
    }
  ]
}
```

