
# Account

## Structure

`Account`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Id` | `int` | Required | - |
| `Currency` | `string` | Required | - |
| `Balance` | [`AccountBalance`](../../doc/models/containers/account-balance.md) | Required | This is a container for one-of cases. |
| `IsActive` | `bool` | Required | - |
| `SandboxMode` | `bool` | Required | - |

## Example (as JSON)

```json
{
  "id": 555,
  "currency": "USD",
  "balance": 100828.4848,
  "isActive": true,
  "sandboxMode": true
}
```

