
# Place Order Request 1

## Structure

`PlaceOrderRequest1`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `RequestId` | `string` | Required | - |
| `AccountId` | `int` | Required | - |
| `Products` | [`List<Product3>`](../../doc/models/product-3.md) | Required | - |

## Example (as JSON)

```json
{
  "RequestId": "{{$guid}}",
  "AccountId": 307,
  "Products": [
    {
      "Sku": "TBRAND-AE",
      "Quantity": 1,
      "Value": 10
    }
  ]
}
```

