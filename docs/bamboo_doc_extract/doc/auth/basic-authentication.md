
# Basic Authentication



Documentation for accessing and setting credentials for basic.

## Auth Credentials

| Name | Type | Description | Setter | Getter |
|  --- | --- | --- | --- | --- |
| username | `string` | - | `Username` | `Username` |
| password | `string` | - | `Password` | `Password` |



**Note:** Auth credentials can be set using `BasicAuthCredentials` in the client builder and accessed through `BasicAuthCredentials` method in the client instance.

## Usage Example

### Client Initialization

You must provide credentials in the client as shown in the following code snippet.

```csharp
BambooClientIntegrationDocumentationV2Client client = new BambooClientIntegrationDocumentationV2Client.Builder()
    .BasicAuthCredentials(
        new BasicAuthModel.Builder(
            "username",
            "password"
        )
        .Build())
    .Build();
```


