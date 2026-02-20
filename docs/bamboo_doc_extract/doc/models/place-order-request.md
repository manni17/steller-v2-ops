
# Place Order Request

## Structure

`PlaceOrderRequest`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `RequestId` | `string` | Required | - |
| `AccountId` | `int` | Required | - |
| `Products` | [`List<Product1>`](../../doc/models/product-1.md) | Required | - |

## Example (as JSON)

```json
{
  "RequestId": "{{$guid}}",
  "AccountId": 0,
  "Products": [
    {
      "ProductId": 0,
      "Quantity": 1,
      "Value": 1
    }
  ]
}
```

