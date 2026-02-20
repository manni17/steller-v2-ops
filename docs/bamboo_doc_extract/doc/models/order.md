
# Order

## Structure

`Order`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `ClientName` | `string` | Required | - |
| `OrderNumber` | `string` | Required | - |
| `OrderDate` | `string` | Required | - |
| `ClientReferenceNumber` | `string` | Required | - |
| `Channel` | `string` | Required | - |
| `ProductName` | `string` | Required | - |
| `Denomination` | `string` | Required | - |
| `Quantity` | `int` | Required | - |
| `UnitPrice` | `double` | Required | - |
| `SubTotal` | `double` | Required | - |
| `Total` | `double` | Required | - |
| `AccountCurrency` | `string` | Required | - |
| `Status` | `string` | Required | - |

## Example (as JSON)

```json
{
  "clientName": "Bamboo Tech - Testing",
  "orderNumber": "O-06761537",
  "orderDate": "2024-05-15T00:00:00",
  "clientReferenceNumber": "97018cf7-98f5-40fc-a142-711d43a72e17",
  "channel": "Sandbox",
  "productName": "Steam 10 USD BH",
  "denomination": "USD 10",
  "quantity": 1,
  "unitPrice": 9.47,
  "subTotal": 9.47,
  "total": 9.47,
  "accountCurrency": "USD",
  "status": "Succeeded"
}
```

