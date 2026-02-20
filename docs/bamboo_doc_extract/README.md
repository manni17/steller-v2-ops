
# Getting Started with Bamboo Client Integration (documentation) - V2

## Introduction

Our platform stands as the primary channel through which customers can place orders, and track, and manage stored-value products. API endpoints are restricted to authorized customers, requiring setup and provision of credentials by our team. Customers with the requisite credentials can then employ Basic Authentication to access the API endpoints.

**Environments:**

- **Production** Host is: [https://api.bamboocardportal.com](https://)

- **Sandbox**: The host is the same, but the credentials will differ, resulting in distinct outcomes.

**Authentication:**

- **Authentication Mechanism:**
  
  - The Bamboo Client API operates on Basic Authentication.

- **Client Onboarding Process:**
  
  - Upon initiating the client onboarding procedure, Bamboo systematically enrols the client within the system.

- **Email Invitation:**
  
  - A formal email invitation is dispatched to the specified email address as part of the onboarding process.

- **User Login:**
  
  - The Client User can then log in using the link provided in the invitation.

- **Environment Credentials Generation:**
  
  - This initiates the process of generating essential environmental credentials once logged in to the portal.

- **Execution Steps:**
  
  - These steps can be seamlessly executed by following the detailed instructions outlined below.

**Flow:**

a. Navigate to the Dashboard and locate your profile button situated at the bottom left corner.

b. Click on "API Settings," triggering a pop-up window.

c. Within the pop-up window, select **"Generate API Key,**" prompting the display of another pop-up.

d. Click the "Generate Credentials" button.  
e. A default Sandbox Account will be established, and the Client User can produce the Sandbox credential from the aforementioned API Settings Panel.

_**Note:**_ _Upon successfully generating new credentials, your old credentials will no longer be valid._

- **Credential Persistence:**
  
  - Credentials possess indefinite validity and do not expire over time.

- **User-Controlled Regeneration:**
  
  - Users maintain the capability to regenerate credentials at their discretion.

- **ClientId Consistency:**
  
  - When credentials are regenerated, the ClientId remains unchanged, ensuring continuity. Only the ClientSecret undergoes modification.

- **Security Measures:**
  
  - To securely access and update data through the Bamboo API, it is imperative to whitelist external IP addresses, as depicted in Figure 1.4.

- Distinct credentials are required for each environment.

**API Limitations (Live and Sandbox Environments):**

- **Get Catalog:**
  
  - Our server permits 2 requests per hour for retrieving the catalogue. (Product mapping to your end is required.)
  
  - Catalog prices, if applicable, are updated on an hourly basis.
  
  - NOTE: Please refrain from utilizing this endpoint as there exists a superior version listed below.

- **Get Order:**
  
  - For fetching orders, our server allows 120 requests per minute.

- **Place Order:**
  
  - Placing orders is restricted to 2 requests per second on our server.

- **Exchange Rate:**
  
  - Our server authorizes 20 requests per minute for obtaining exchange rates.
  
  - Exchange rates are updated on an hourly basis.

- **Transactions:**
  
  - The system allows 4 requests per hour for transaction-related queries.

- **Other APIs:**
  
  - Default allowance of 1 request per second for all the other APIs.

## Building

The generated code uses the Newtonsoft Json.NET NuGet Package. If the automatic NuGet package restore is enabled, these dependencies will be installed automatically. Therefore, you will need internet access for build.

* Open the solution (BambooClientIntegrationDocumentationV2.sln) file.

Invoke the build process using Ctrl + Shift + B shortcut key or using the Build menu as shown below.

The build process generates a portable class library, which can be used like a normal class library. More information on how to use can be found at the MSDN Portable Class Libraries documentation.

The supported version is **.NET Standard 2.0**. For checking compatibility of your .NET implementation with the generated library, [click here](https://dotnet.microsoft.com/en-us/platform/dotnet-standard#versions).

## Installation

The following section explains how to use the BambooClientIntegrationDocumentationV2.Standard library in a new project.

### 1. Starting a new project

For starting a new project, right click on the current solution from the solution explorer and choose `Add -> New Project`.

![Add a new project in Visual Studio](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=addProject)

Next, choose `Console Application`, provide `TestConsoleProject` as the project name and click OK.

![Create a new Console Application in Visual Studio](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=createProject)

### 2. Set as startup project

The new console project is the entry point for the eventual execution. This requires us to set the `TestConsoleProject` as the start-up project. To do this, right-click on the `TestConsoleProject` and choose `Set as StartUp Project` form the context menu.

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=setStartup)

### 3. Add reference of the library project

In order to use the `BambooClientIntegrationDocumentationV2.Standard` library in the new project, first we must add a project reference to the `TestConsoleProject`. First, right click on the `References` node in the solution explorer and click `Add Reference...`

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=addReference)

Next, a window will be displayed where we must set the `checkbox` on `BambooClientIntegrationDocumentationV2.Standard` and click `OK`. By doing this, we have added a reference of the `BambooClientIntegrationDocumentationV2.Standard` project into the new `TestConsoleProject`.

![Creating a project reference](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=createReference)

### 4. Write sample code

Once the `TestConsoleProject` is created, a file named `Program.cs` will be visible in the solution explorer with an empty `Main` method. This is the entry point for the execution of the entire solution. Here, you can add code to initialize the client library and acquire the instance of a Controller class. Sample code to initialize the client library and using Controller methods is given in the subsequent sections.

![Adding a project reference](https://apidocs.io/illustration/cs?workspaceFolder=Bamboo%20Client%20Integration%20%28documentation%29%20-%20V2-CSharp&workspaceName=BambooClientIntegrationDocumentationV2&projectName=BambooClientIntegrationDocumentationV2.Standard&rootNamespace=BambooClientIntegrationDocumentationV2.Standard&step=addCode)

## Test the SDK

The generated SDK also contain one or more Tests, which are contained in the Tests project. In order to invoke these test cases, you will need `NUnit 3.0 Test Adapter Extension` for Visual Studio. Once the SDK is complied, the test cases should appear in the Test Explorer window. Here, you can click `Run All` to execute these test cases.

## Initialize the API Client

**_Note:_** Documentation for the client can be found [here.](doc/client.md)

The following parameters are configurable for the API Client:

| Parameter | Type | Description |
|  --- | --- | --- |
| `Environment` | `Environment` | The API environment. <br> **Default: `Environment.Production`** |
| `Timeout` | `TimeSpan` | Http client timeout.<br>*Default*: `TimeSpan.FromSeconds(100)` |
| `BasicAuthCredentials` | [`BasicAuthCredentials`](doc/auth/basic-authentication.md) | The Credentials Setter for Basic Authentication |

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

## Authorization

This API uses the following authentication schemes.

* [`basic (Basic Authentication)`](doc/auth/basic-authentication.md)

## List of APIs

* [Integration V1](doc/controllers/integration-v1.md)
* [Integration V2](doc/controllers/integration-v2.md)
* [Integration V2 1](doc/controllers/integration-v2-1.md)

## Classes Documentation

* [Utility Classes](doc/utility-classes.md)
* [HttpRequest](doc/http-request.md)
* [HttpResponse](doc/http-response.md)
* [HttpStringResponse](doc/http-string-response.md)
* [HttpContext](doc/http-context.md)
* [HttpClientConfiguration](doc/http-client-configuration.md)
* [HttpClientConfiguration Builder](doc/http-client-configuration-builder.md)
* [IAuthManager](doc/i-auth-manager.md)
* [ApiException](doc/api-exception.md)

