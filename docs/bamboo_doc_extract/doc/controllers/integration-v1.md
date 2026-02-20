# Integration V1

```csharp
IntegrationV1Controller integrationV1Controller = client.IntegrationV1Controller;
```

## Class Name

`IntegrationV1Controller`

## Methods

* [Get Catalog](../../doc/controllers/integration-v1.md#get-catalog)
* [Get Accounts](../../doc/controllers/integration-v1.md#get-accounts)
* [Place Order](../../doc/controllers/integration-v1.md#place-order)
* [Get Order](../../doc/controllers/integration-v1.md#get-order)
* [Exchange Rates](../../doc/controllers/integration-v1.md#exchange-rates)
* [Transactions](../../doc/controllers/integration-v1.md#transactions)
* [Orders](../../doc/controllers/integration-v1.md#orders)
* [Notification](../../doc/controllers/integration-v1.md#notification)
* [Notification 1](../../doc/controllers/integration-v1.md#notification-1)


# Get Catalog

Clients are required to include valid credentials in the Authorization header for every GET request. Utilizing the GET endpoint /api/integration/v1.0/catalog allows you to retrieve a comprehensive list of available brands and products. A successful GET request will yield a response body containing the details of each brand, providing valuable information about the available offerings.

### Example response:

```json
{
    "brands": [
        {
            "name": "string",
            "countryCode": "string",
            "currencyCode": "string",
            "description": "string",
            "disclaimer": "string",
            "redemptionInstructions": "string",
            "terms": "string",
            "logoUrl": "string",
            "modifiedDate": "2020-09-23T15:01:00.8409786",
            "products": [
                {
                    "id": 1,
                    "name": "string",
                    "minFaceValue": 1.00,
                    "maxFaceValue": 1000.00,
                    "count": 1000,
                    "price": {
                        "min": 1.00,
                        "max": 1000.00,
                        "currencyCode": "USD"
                    },
                    "modifiedDate": "2020-09-23T15:01:01.00000"
                }
            ]
        }
    ]
}

```

**Response Specifications:**

1. **Description Field:**
   
   - The description field is subject to being empty.

2. **Disclaimer Field:**
   
   - The disclaimer field may be left empty.

3. **Redemption Instructions:**
   
   - The RedemptionInstructions field is open to being empty.

4. **Terms Field:**
   
   - The Terms field is susceptible to being empty.

5. **Face Value Details:**
   
   - For products with a range denominations:
     
     - Products\[\].MinFaceValue indicates the minimum face value of the card.
     
     - Products\[\].MaxFaceValue indicates the maximum face value of the card.
   
   - For products with a fixed value:
     
     - MinFaceValue and MaxFaceValue are equivalent.

6. **Card Quantity Information:**
   
   - Products\[\].Count signifies the number of cards available for purchase.
     
     - If the Count is null, the card amount is unrestricted.
     
     - If the Count is 0, the product is currently out of stock.

_We are going to discontinue this endpoint on the 30 of September, and then v2 will be here instead._

## Sandbox usage.

**Sandbox Usage Guidelines:**

1. **Credential Distinction:**
   
   - Sandbox utilizes distinct credentials separate from the Live environment.

2. **Catalogue Consistency:**
   
   - The Sandbox mirrors the Live environment's catalogue for consistent testing.

3. **Current Account Distinction:**
   
   - Sandbox employs different current accounts compared to the Live environment, and topping up should be done separately.

4. **Order Channel Identification:**
   
   - Orders generated in the Sandbox will be associated with the "sandbox" channel (order-type).

5. **Sandbox Reports and Transactions:**
   
   - Reports and transactions in the Sandbox environment will exclusively return information related to Sandbox activities.

```csharp
GetCatalogAsync()
```

## Response Type

[`Task<Models.GetCatalog>`](../../doc/models/get-catalog.md)

## Example Usage

```csharp
try
{
    GetCatalog result = await integrationV1Controller.GetCatalogAsync();
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```


# Get Accounts

User must include valid credentials in the Authorization header of each GET request. The GET /api/integration/v1.0/accounts endpoint lets you retrieve the list of your accounts. A successful GET request returns a list of available accounts in the response body with the information for each account. Use one of the account ids in order to say what account to use when placing the order.

### Example response:

```json
{
    "accounts": [
        {
            "id": 1,
            "currency": "string",
            "balance": 100.00,
            "isActive": true,
            "sandboxMode": true
        }
    ]
}

```

```csharp
GetAccountsAsync()
```

## Response Type

[`Task<Models.GetAccounts>`](../../doc/models/get-accounts.md)

## Example Usage

```csharp
try
{
    GetAccounts result = await integrationV1Controller.GetAccountsAsync();
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```


# Place Order

User have to include valid credentials in the Authorization header of each POST request. The POST /api/integration/v1.0/orders/checkout endpoint lets you place an order for gift cards. A successful POST request returns requested, that should be used to get cards

**Request Remarks**

- **RequestId Uniqueness:**
  
  - RequestId should be GUID
  
  - Ensure that the RequestId is unique for each client, avoiding duplication.

- **Valid AccountId:**
  
  - The AccountId must correspond to an active account within the system.

- **ProductId Validation:**
  
  - The ProductId must align with the unique identifier of a product from the catalogue.

- **Value Range Validation:**
  
  - The 'Value' parameter must fall within the inclusive range of MinFaceValue to MaxFaceValue. Note that this range is based on the face value of the product, not its price.

### Example response:

```json
{
    "97018cf7-98f5-40fc-a142-711d43a72e17"
}

```

**Order Request Remarks:**

1. **AccountId Source:**
   
   - Retrieve the AccountId through the Get Accounts API call. Choose one of the account IDs provided in the Get Accounts response.

2. **Duplicate Order Detection:**
   
   - If an order with the specified RequestId already exists, an error will occur indicating the order's existence.

3. **Immediate Order Processing:**
   
   - Upon request submission, orders will be processed immediately. There's no option to create an order without processing it.

4. **Value Parameter Validity:**
   
   - The 'Value' parameter in the request denotes the face value and must be valid within the range of MinFaceValue and MaxFaceValue.

5. **Currency Discrepancy Handling:**
   
   - When purchasing from an account with a currency different from the listed product price in the catalogue, an exchange rate will be applied.

6. **Exchange Rate Application Example:**
   
   - Example scenario:
     1. Desired purchase: a product priced at 9.5 EUR with a face value of 10.
     
     2. Using a USD account.
     
     3. Exchange rate from EUR to USD: 1.10.
     
     4. Calculated product price: 9.5 \* 1.10 = 10.45 USD.
     
     5. The deducted amount will be in USD from your account.

### **Limits**:

- **Maximum Cards per Order:**
  
  - The maximum allowable number of cards in a single order is set at 500. Any attempt to exceed this limit will result in a response status code of 400.

- **Order Placement Delay:**
  
  - A delay of 500 milliseconds must be observed between consecutive Place Order calls.

- **Maximum Orders for the Same Product:**
  
  - The system imposes a limit of 6 orders for the same product within 1 minute.

- **Maximum Quantity per Order:**
  
  - Each order is constrained by a maximum quantity of 500 cards.

| **Reason (400 Response)** | **Message** |
| --- | --- |
| WrongAccount | The account ID being used is incorrect. Please ensure you are using the valid account ID as indicated in the API response. |
| InsufficientBalance | Your account balance is insufficient for this transaction. Kindly add funds to your account and attempt the transaction again. |
| InvalidProductIds | The product IDs you are providing are invalid. Please use valid IDs from your catalog. |
| InvalidProduct | This product cannot be ordered. Please try again with a different product. |
| InvalidDenomination | The denomination value you are providing is invalid. Please pass a value that matches the minimum or maximum product value. |
| NoProducts | The request must contain products to buy. |
| OrderAlreadyExists | This order already exists.  <br>Please try again with a new requestId. |
| ClientCatalogNotExist | The catalogue you are using does not exist. |
| OrderNotFound | The order you are trying to get is not found. |
| CardsLimitExceeded | There is a limit on the number of cards you can purchase for this product in a single call. The maximum limit is 500 cards. |
| ProductIsOutOfStock | The product is out of stock. |
| ClientPriceInvalid | If the client price is not valid, contact your account manager. |
| UserNotEnabled | This user is not enabled yet. |

```csharp
PlaceOrderAsync(
    Models.PlaceOrderRequest body)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `body` | [`PlaceOrderRequest`](../../doc/models/place-order-request.md) | Body, Required | - |

## Response Type

`Task<string>`

## Example Usage

```csharp
PlaceOrderRequest body = new PlaceOrderRequest
{
    RequestId = "{{$guid}}",
    AccountId = 0,
    Products = new List<Product1>
    {
        new Product1
        {
            ProductId = 0,
            Quantity = 1,
            MValue = 1,
        },
    },
};

try
{
    string result = await integrationV1Controller.PlaceOrderAsync(body);
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response

```
"\"71ac2817-e51a-438a-bb7b-5ffdda23603c\""
```


# Get Order

User have to include valid credentials in the Authorization header of each GET request. The GET/api/integration/v1.0/orders/checkout/{requestId} endpoint lets you get the order. A successful GET request returns order information with cards requested in the response body with the information for each product requested.

### Example response:

**Order Status Explanation:**

- **Created:**
  
  - The initial status is assigned to an order upon its creation in the system. At this stage, the order is ready for processing.

- **Processing:**
  
  - Indicates that the system has accepted the order, and it is in progress.

- **Pending:**
  
  - The order is awaiting further action or approval before it can proceed to the next stage. This status is often temporary.

- **Succeeded:**
  
  - Signifies that the order has been successfully completed, and the requested operation or transaction has been carried out without any issues.

- **Failed:**
  
  - Indicates that the order encountered an error or faced an obstacle during processing, preventing successful completion.

- **PartialFailed:**
  
  - A hybrid status indicates that certain aspects of the order were successful, while others encountered issues or failures. This status provides a nuanced view of the order's overall outcome.

**Remarks:**

- _If you are getting the status Pending, this means that the order is currently being processed, wait until you get another status._

```json
{
    "orderId": 1,
    "requestId": "97018cf7-98f5-40fc-a142-711d43a72e17",
    "items": [
        {
            "brandCode": "string",
            "productId": 1,
            "productFaceValue": 1,
            "quantity": 1,
            "pictureUrl": "string",
            "countryCode": "string",
            "currencyCode": "string",
            "status" : "string"
            "cards": [
                {
                    "id": 1,
                    "serialNumber": "string",
                    "cardCode": "string",
                    "pin": "string",
                    "expirationDate": "string",
                    "status": "string"
                }
            ]
        }
    ],
    "status": "string",
    "createdDate": "2020-09-25T14:04:50.00000",
    "total": 1,
    "errorMessage": "string"
    "orderType": "Sandbox",
    "currency": "USD"
}

```

### Response Remarks:

**Response Remarks:**

- **Purchase Outcome:**
  
  - In some instances, not all cards may be successfully purchased. In such cases, a failure message is returned specifically for the cards that were not acquired.

- **Order Status, Order Items Status Indicators:**
  
  - The order can have the following statuses:
    
    - **Succeeded:** Successfully processed.
    
    - **Created:** Order is created but processing has not commenced.
    
    - **Failed:** The order encountered an issue, and no cards were purchased.
    
    - **PartialFailed:** Some cards were bought, but others encountered issues.

- **Total Amount Charged:**
  
  - The "Total" field in the response indicates the total amount of money charged from the account in its respective currency.

**Card Status Explanation:**

- **Paused:**
  
  - Cards in a paused status are awaiting retrieval from different channels.

- **Sold:**
  
  - Cards that have been successfully purchased.

- **Failed:**
  
  - Cards that encountered failures during the purchase attempt
  
  - and no charges will be applied to them.

- **Created:**
  
  - Indicates that order processing for the respective card has not started yet.

- **Processing:**
  
  - Denotes that the system is currently processing the card within the order.

```csharp
GetOrderAsync(
    string id)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `id` | `string` | Template, Required | - |

## Response Type

[`Task<Models.GetOrder>`](../../doc/models/get-order.md)

## Example Usage

```csharp
string id = "fb855c6a-f8de-4a5a-a87b-d2d7a7a4d049";
try
{
    GetOrder result = await integrationV1Controller.GetOrderAsync(id);
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "orderId": 6779294,
  "requestId": "71ac2817-e51a-438a-bb7b-5ffdda23603c",
  "items": [
    {
      "brandCode": null,
      "productId": 439685,
      "productFaceValue": 10,
      "quantity": 2,
      "pictureUrl": null,
      "countryCode": "BH",
      "currencyCode": "USD",
      "cards": [
        {
          "id": 24153984,
          "serialNumber": "4005c08f-1aeb-4527-8eb0-ccd1c745f467",
          "cardCode": "cb05747ed9fe497b92c80f10cf8406d7",
          "pin": "392",
          "expirationDate": "2025-05-16T04:36:35.3922677",
          "status": "Sold"
        },
        {
          "id": 24153985,
          "serialNumber": "29744102-1e84-48e0-a440-8b3a5c04b4d2",
          "cardCode": "7dffe979e54c49f692e7894eb91407d1",
          "pin": "392",
          "expirationDate": "2025-05-16T04:36:35.3922628",
          "status": "Sold"
        }
      ]
    }
  ],
  "status": "Succeeded",
  "createdDate": "2024-05-16T04:36:35.3673422",
  "total": 18.9401,
  "errorMessage": null,
  "orderType": "Sandbox",
  "currency": "USD"
}
```


# Exchange Rates

**Exchange Rate Retrieval Guidelines:**

- **Credential Inclusion:**
  
  - Ensure the inclusion of valid credentials in the Authorization header for each GET request.

- **Endpoint for Exchange Rates:**
  
  - Utilize the GET endpoint to retrieve the list of exchange rates used in the system.

- **Query Parameters:**
  
  - If a request is sent without query parameters, the endpoint returns a list of all exchange rates based on USD.

- **Base Currency Parameter:**
  
  - If a request includes the baseCurrency parameter, the endpoint will return a list of all exchange rates based on the specified base currency.

- **Base and Currency Parameters:**
  
  - If a request contains both baseCurrency and currency parameters, the endpoint will specifically return the exchange rate between the base currency and the specified currency.

### Example response:

```json
{
    "baseCurrencyCode": "USD",
    "rates": [
        {
            "value": 0.8714830000,
            "currencyCode": "EUR"
        }
    ]
}

```

```csharp
ExchangeRatesAsync(
    string baseCurrency,
    string currency)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `baseCurrency` | `string` | Query, Required | - |
| `currency` | `string` | Query, Required | - |

## Response Type

[`Task<Models.ExchangeRates>`](../../doc/models/exchange-rates.md)

## Example Usage

```csharp
string baseCurrency = "AED";
string currency = "INR";
try
{
    ExchangeRates result = await integrationV1Controller.ExchangeRatesAsync(
        baseCurrency,
        currency
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

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


# Transactions

When the the user calls the aforementioned API providing valid dates ( the range containing data ) and a valid username and password in Basic Auth, an Object in the following format will be returned.

```json
"startDate": "2022-10-01T00:00:00",
"endDate": "2022-11-20T23:59:59",
"clients": [
        {
            "clientName": "string",
            "transactions": [
                {
                    "orderId": 3824312,
                    "requestId": "string",
                    "productId": 483230,
                    "orderNumber": "string",
                    "orderTotal": {
                        "value": 108.5100,
                        "currencyCode": "USD"
                    },
                    "orderTotalForSelectedProducts": {
                        "value": 108.5100,
                        "currencyCode": "USD"
                    },
                    "currencyCode": null,
                    "orderDate": "2023-11-17T07:57:18.8519922",
                    "transactionId": "3752352 ",
                    "transactionType": "Order",
                    "comment": null,
                    "notes": null,
                    "availableBalance": {
                        "value": 99875.8400,
                        "currencyCode": "USD"
                    },
                    "transactionAmount": {
                        "value": 108.5100,
                        "currencyCode": "USD"
                    },
                    "transactionDate": "2023-11-17T07:57:18.8519922",
                    "orderItems": [
                        {
                            "createdDate": "0001-01-01T00:00:00",
                            "productName": "string",
                            "brandName": "string",
                            "brandCurrencyCode": "EUR",
                            "brandWithDenominationCurrency": "string",
                            "denomination": {
                                "value": 10.0000,
                                "currencyCode": "EUR"
                            },
                            "clientDiscountType": null,
                            "clientDiscountValue": 0.0000,
                            "clientTransactionFeeType": null,
                            "clientTransactionFeeValue": 0.0000,
                            "clientPrice": {
                                "value": 108.5100,
                                "currencyCode": "USD"
                            },
                            "estimatedPrice": {
                                "value": 108.5100,
                                "currencyCode": "USD"
                            },
                            "quantity": 10,
                            "clientTotal": 108.5100,
                            "clientExchangeRate": {
                                "fromCurrency": "EUR",
                                "toCurrency": "USD",
                                "exchangeRate": 0.0
                            }
                        }
                    ],
                    "clientName": null,
                    "orderType": null,
                    "xeroCreditNoteNumber": null
                }
            ]
        }
    ]
}

```

```csharp
TransactionsAsync(
    string startDate,
    string endDate)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `startDate` | `string` | Query, Required | - |
| `endDate` | `string` | Query, Required | - |

## Response Type

[`Task<Models.Transactions>`](../../doc/models/transactions.md)

## Example Usage

```csharp
string startDate = "2022-10-01";
string endDate = "2022-11-20";
try
{
    Transactions result = await integrationV1Controller.TransactionsAsync(
        startDate,
        endDate
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "startDate": "2024-05-15T00:00:00",
  "endDate": "2024-05-16T23:59:59",
  "clients": [
    {
      "clientName": "Bamboo Tech - Testing",
      "transactions": [
        {
          "orderId": 6761537,
          "requestId": "97018cf7-98f5-40fc-a142-711d43a72e17",
          "productId": 439685,
          "orderNumber": "O-06761537",
          "orderTotal": {
            "value": 9.47,
            "currencyCode": "USD"
          },
          "orderTotalForSelectedProducts": {
            "value": 9.47,
            "currencyCode": "USD"
          },
          "currencyCode": null,
          "orderDate": "2024-05-15T04:29:41.3062431",
          "transactionId": "6677709 ",
          "transactionType": "Order",
          "comment": null,
          "notes": null,
          "availableBalance": {
            "value": 100828.4848,
            "currencyCode": "USD"
          },
          "transactionAmount": {
            "value": 9.47,
            "currencyCode": "USD"
          },
          "transactionDate": "2024-05-15T04:29:41.3062431",
          "orderItems": [
            {
              "createdDate": "0001-01-01T00:00:00",
              "productName": "Steam 10 USD BH",
              "brandName": "Steam Bahrain",
              "brandCurrencyCode": "USD",
              "brandWithDenominationCurrency": "Steam Bahrain USD",
              "denomination": {
                "value": 10,
                "currencyCode": "USD"
              },
              "clientDiscountType": "Percentage",
              "clientDiscountValue": 5.2995,
              "clientTransactionFeeType": "Fixed",
              "clientTransactionFeeValue": 0,
              "clientPrice": {
                "value": 9.47,
                "currencyCode": "USD"
              },
              "estimatedPrice": {
                "value": 9.47,
                "currencyCode": "USD"
              },
              "quantity": 1,
              "clientTotal": 9.47,
              "clientExchangeRate": {
                "fromCurrency": "USD",
                "toCurrency": "USD",
                "exchangeRate": 0
              }
            }
          ],
          "clientName": null,
          "orderType": null,
          "xeroCreditNoteNumber": null
        },
        {
          "orderId": 6779294,
          "requestId": "71ac2817-e51a-438a-bb7b-5ffdda23603c",
          "productId": 439685,
          "orderNumber": "O-06779294",
          "orderTotal": {
            "value": 18.9401,
            "currencyCode": "USD"
          },
          "orderTotalForSelectedProducts": {
            "value": 18.94,
            "currencyCode": "USD"
          },
          "currencyCode": null,
          "orderDate": "2024-05-16T04:36:35.4268366",
          "transactionId": "6695293 ",
          "transactionType": "Order",
          "comment": null,
          "notes": null,
          "availableBalance": {
            "value": 100809.5447,
            "currencyCode": "USD"
          },
          "transactionAmount": {
            "value": 18.9401,
            "currencyCode": "USD"
          },
          "transactionDate": "2024-05-16T04:36:35.4268366",
          "orderItems": [
            {
              "createdDate": "0001-01-01T00:00:00",
              "productName": "Steam 10 USD BH",
              "brandName": "Steam Bahrain",
              "brandCurrencyCode": "USD",
              "brandWithDenominationCurrency": "Steam Bahrain USD",
              "denomination": {
                "value": 10,
                "currencyCode": "USD"
              },
              "clientDiscountType": "Percentage",
              "clientDiscountValue": 5.2995,
              "clientTransactionFeeType": "Fixed",
              "clientTransactionFeeValue": 0,
              "clientPrice": {
                "value": 18.9401,
                "currencyCode": "USD"
              },
              "estimatedPrice": {
                "value": 18.94,
                "currencyCode": "USD"
              },
              "quantity": 2,
              "clientTotal": 18.94,
              "clientExchangeRate": {
                "fromCurrency": "USD",
                "toCurrency": "USD",
                "exchangeRate": 0
              }
            }
          ],
          "clientName": null,
          "orderType": null,
          "xeroCreditNoteNumber": null
        }
      ]
    }
  ]
}
```


# Orders

- To access the orders history, you must include valid credentials in the Authorization header of each GET request.

- Utilize the GET/api/integration/v1.0/orders endpoint.

- Upon successful execution of the GET request, the response body will contain the requested orders information, with details for each order included.

```json
[
    {
        "clientName": "string",
        "orderNumber": "string",
        "orderDate": "2022-11-17T00:00:00",
        "clientReferenceNumber": "string",
        "channel": "string",
        "productName": "string",
        "denomination": "EUR 10",
        "quantity": 10,
        "unitPrice": 10.8510,
        "subTotal": 108.5100,
        "total": 108.5100,
        "accountCurrency": "USD",
        "status": "Succeeded"
    },
]

```

```csharp
OrdersAsync(
    string startDate,
    string endDate)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `startDate` | `string` | Query, Required | - |
| `endDate` | `string` | Query, Required | - |

## Response Type

[`Task<List<Models.Order>>`](../../doc/models/order.md)

## Example Usage

```csharp
string startDate = "2022-10-01";
string endDate = "2022-11-20";
try
{
    List<Order> result = await integrationV1Controller.OrdersAsync(
        startDate,
        endDate
    );
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
[
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
  },
  {
    "clientName": "Bamboo Tech - Testing",
    "orderNumber": "O-06779294",
    "orderDate": "2024-05-16T00:00:00",
    "clientReferenceNumber": "71ac2817-e51a-438a-bb7b-5ffdda23603c",
    "channel": "Sandbox",
    "productName": "Steam 10 USD BH",
    "denomination": "USD 10",
    "quantity": 2,
    "unitPrice": 9.47,
    "subTotal": 18.94,
    "total": 18.9401,
    "accountCurrency": "USD",
    "status": "Succeeded"
  }
]
```


# Notification

This API enables you to configure the notification URL in your system. To enhance security, you have the option to set a secretKey and validate it on your end. Please note that setting the notification URL to empty will disable this feature.

#### **Notification Callback Response**

When an order reaches the "complete" status (whether it's Succeeded, Failed, or PartialFailed), our API will initiate a POST request to your configured endpoint. The request will include the following parameters. After receiving this request, you will need to fetch the order details using the Get Order endpoint.

```json
{
  "orderId": 12345 ,
  "status": "succeeded",
  "totalCards": 10,
  "createdOn": "datetime order create date", 
  "completedOn": "datetime order completed date",
  "secretKey": "Merchent configured secret key",
  "requestId": "GUID Merchent Identifier"
}

```

```csharp
NotificationAsync(
    Models.NotificationRequest body)
```

## Parameters

| Parameter | Type | Tags | Description |
|  --- | --- | --- | --- |
| `body` | [`NotificationRequest`](../../doc/models/notification-request.md) | Body, Required | - |

## Response Type

[`Task<Models.Notification>`](../../doc/models/notification.md)

## Example Usage

```csharp
NotificationRequest body = new NotificationRequest
{
    NotificationUrl = "merchant end point callback url",
    NotificationUrlSandbox = "merchant sandbox end point callback url",
    SecretKey = "merchant end point verification secret key",
};

try
{
    Notification result = await integrationV1Controller.NotificationAsync(body);
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "notificationUrl": "https://yoururl/order/notification",
  "notificationUrlSandbox": "https://yoururl/order/notification",
  "secretKey": "76d247c4-015d-470e-9ff6-5c1c873d1e0d"
}
```


# Notification 1

This API returns the configured notification URL in your system along with the associated secret key, which will be validated on your side.

### Example response:

```json
{
    "notificationUrl": "your endpoint callback url",
    "notificationUrlSandbox": "merchant sandbox end point callback url",
    "secretKey": "your endpoint verification secret key",
}

```

```csharp
Notification1Async()
```

## Response Type

[`Task<Models.Notification>`](../../doc/models/notification.md)

## Example Usage

```csharp
try
{
    Notification result = await integrationV1Controller.Notification1Async();
}
catch (ApiException e)
{
    // TODO: Handle exception here
    Console.WriteLine(e.Message);
}
```

## Example Response *(as JSON)*

```json
{
  "notificationUrl": "https://yoururl/order/notification",
  "notificationUrlSandbox": "https://yoururl/order/notification",
  "secretKey": "76d247c4-015d-470e-9ff6-5c1c873d1e0d"
}
```

