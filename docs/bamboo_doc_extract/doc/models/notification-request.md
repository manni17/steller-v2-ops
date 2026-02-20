
# Notification Request

## Structure

`NotificationRequest`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `NotificationUrl` | `string` | Required | - |
| `NotificationUrlSandbox` | `string` | Required | - |
| `SecretKey` | `string` | Required | - |

## Example (as JSON)

```json
{
  "notificationUrl": "merchant end point callback url",
  "notificationUrlSandbox": "merchant sandbox end point callback url",
  "secretKey": "merchant end point verification secret key"
}
```

