
# Client Class Documentation

The following parameters are configurable for the API Client:

| Parameter | Type | Description |
|  --- | --- | --- |
| `Environment` | `Environment` | The API environment. <br> **Default: `Environment.Production`** |
| `Timeout` | `TimeSpan` | Http client timeout.<br>*Default*: `TimeSpan.FromSeconds(100)` |
| `BasicAuthCredentials` | [`BasicAuthCredentials`](auth/basic-authentication.md) | The Credentials Setter for Basic Authentication |

The API client can be initialized as follows:

```csharp
BambooClientIntegrationDocumentationV2Client client = new BambooClientIntegrationDocumentationV2Client.Builder()
    .BasicAuthCredentials(
        new BasicAuthModel.Builder(
            "username",
            "password"
        )
        .Build())
    .Environment(BambooClientIntegrationDocumentationV2.Standard.Environment.Production)
    .Build();
```

## Bamboo Client Integration (documentation) - V2Client Class

The gateway for the SDK. This class acts as a factory for the Controllers and also holds the configuration of the SDK.

### Controllers

| Name | Description |
|  --- | --- |
| IntegrationV1Controller | Gets IntegrationV1Controller controller. |
| IntegrationV2Controller | Gets IntegrationV2Controller controller. |
| IntegrationV21Controller | Gets IntegrationV21Controller controller. |

### Properties

| Name | Description | Type |
|  --- | --- | --- |
| HttpClientConfiguration | Gets the configuration of the Http Client associated with this client. | [`IHttpClientConfiguration`](http-client-configuration.md) |
| Timeout | Http client timeout. | `TimeSpan` |
| Environment | Current API environment. | `Environment` |
| BasicAuthCredentials | Gets the credentials to use with BasicAuth. | [`IBasicAuthCredentials`](auth/basic-authentication.md) |

### Methods

| Name | Description | Return Type |
|  --- | --- | --- |
| `GetBaseUri(Server alias = Server.Server1)` | Gets the URL for a particular alias in the current environment and appends it with template parameters. | `string` |
| `ToBuilder()` | Creates an object of the Bamboo Client Integration (documentation) - V2Client using the values provided for the builder. | `Builder` |

## Bamboo Client Integration (documentation) - V2Client Builder Class

Class to build instances of Bamboo Client Integration (documentation) - V2Client.

### Methods

| Name | Description | Return Type |
|  --- | --- | --- |
| `HttpClientConfiguration(Action<`[`HttpClientConfiguration.Builder`](http-client-configuration-builder.md)`> action)` | Gets the configuration of the Http Client associated with this client. | `Builder` |
| `Timeout(TimeSpan timeout)` | Http client timeout. | `Builder` |
| `Environment(Environment environment)` | Current API environment. | `Builder` |
| `BasicAuthCredentials(Action<BasicAuthModel.Builder> action)` | Sets credentials for BasicAuth. | `Builder` |

