// <copyright file="IntegrationV1ControllerTest.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using APIMatic.Core.Utilities;
using BambooClientIntegrationDocumentationV2.Standard;
using BambooClientIntegrationDocumentationV2.Standard.Controllers;
using BambooClientIntegrationDocumentationV2.Standard.Exceptions;
using BambooClientIntegrationDocumentationV2.Standard.Http.Client;
using BambooClientIntegrationDocumentationV2.Standard.Http.Response;
using BambooClientIntegrationDocumentationV2.Standard.Models.Containers;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;
using Newtonsoft.Json.Converters;
using NUnit.Framework;

namespace BambooClientIntegrationDocumentationV2.Tests
{
    /// <summary>
    /// IntegrationV1ControllerTest.
    /// </summary>
    [TestFixture]
    public class IntegrationV1ControllerTest : ControllerTestBase
    {
        /// <summary>
        /// Controller instance (for all tests).
        /// </summary>
        private IntegrationV1Controller controller;

        /// <summary>
        /// Setup test class.
        /// </summary>
        [OneTimeSetUp]
        public void SetUpDerived()
        {
            this.controller = this.Client.IntegrationV1Controller;
        }

        /// <summary>
        /// User have to include valid credentials in the Authorization header of each POST request. The POST /api/integration/v1.0/orders/checkout endpoint lets you place an order for gift cards. A successful POST request returns requested, that should be used to get cards
        ///
        ///**Request Remarks**
        ///
        ///- **RequestId Uniqueness:**
        ///    
        ///    - RequestId should be GUID
        ///        
        ///    - Ensure that the RequestId is unique for each client, avoiding duplication.
        ///        
        ///- **Valid AccountId:**
        ///    - The AccountId must correspond to an active account within the system.
        ///        
        ///- **ProductId Validation:**
        ///    - The ProductId must align with the unique identifier of a product from the catalogue.
        ///        
        ///- **Value Range Validation:**
        ///    - The 'Value' parameter must fall within the inclusive range of MinFaceValue to MaxFaceValue. Note that this range is based on the face value of the product, not its price.
        ///        
        ///
        ///### Example response:
        ///
        ///``` json
        ///{
        ///    "97018cf7-98f5-40fc-a142-711d43a72e17"
        ///}
        ///
        /// ```
        ///
        ///**Order Request Remarks:**
        ///
        ///1. **AccountId Source:**
        ///    - Retrieve the AccountId through the Get Accounts API call. Choose one of the account IDs provided in the Get Accounts response.
        ///        
        ///2. **Duplicate Order Detection:**
        ///    - If an order with the specified RequestId already exists, an error will occur indicating the order's existence.
        ///        
        ///3. **Immediate Order Processing:**
        ///    - Upon request submission, orders will be processed immediately. There's no option to create an order without processing it.
        ///        
        ///4. **Value Parameter Validity:**
        ///    - The 'Value' parameter in the request denotes the face value and must be valid within the range of MinFaceValue and MaxFaceValue.
        ///        
        ///5. **Currency Discrepancy Handling:**
        ///    - When purchasing from an account with a currency different from the listed product price in the catalogue, an exchange rate will be applied.
        ///        
        ///6. **Exchange Rate Application Example:**
        ///    - Example scenario:
        ///        1. Desired purchase: a product priced at 9.5 EUR with a face value of 10.
        ///            
        ///        2. Using a USD account.
        ///            
        ///        3. Exchange rate from EUR to USD: 1.10.
        ///            
        ///        4. Calculated product price: 9.5 \* 1.10 = 10.45 USD.
        ///            
        ///        5. The deducted amount will be in USD from your account.
        ///            
        ///
        ///### **Limits**:
        ///
        ///- **Maximum Cards per Order:**
        ///    - The maximum allowable number of cards in a single order is set at 500. Any attempt to exceed this limit will result in a response status code of 400.
        ///        
        ///- **Order Placement Delay:**
        ///    - A delay of 500 milliseconds must be observed between consecutive Place Order calls.
        ///        
        ///- **Maximum Orders for the Same Product:**
        ///    - The system imposes a limit of 6 orders for the same product within 1 minute.
        ///        
        ///- **Maximum Quantity per Order:**
        ///    - Each order is constrained by a maximum quantity of 500 cards.
        ///        
        ///
        ///| **Reason (400 Response)** | **Message** |
        ///| --- | --- |
        ///| WrongAccount | The account ID being used is incorrect. Please ensure you are using the valid account ID as indicated in the API response. |
        ///| InsufficientBalance | Your account balance is insufficient for this transaction. Kindly add funds to your account and attempt the transaction again. |
        ///| InvalidProductIds | The product IDs you are providing are invalid. Please use valid IDs from your catalog. |
        ///| InvalidProduct | This product cannot be ordered. Please try again with a different product. |
        ///| InvalidDenomination | The denomination value you are providing is invalid. Please pass a value that matches the minimum or maximum product value. |
        ///| NoProducts | The request must contain products to buy. |
        ///| OrderAlreadyExists | This order already exists.  <br>Please try again with a new requestId. |
        ///| ClientCatalogNotExist | The catalogue you are using does not exist. |
        ///| OrderNotFound | The order you are trying to get is not found. |
        ///| CardsLimitExceeded | There is a limit on the number of cards you can purchase for this product in a single call. The maximum limit is 500 cards. |
        ///| ProductIsOutOfStock | The product is out of stock. |
        ///| ClientPriceInvalid | If the client price is not valid, contact your account manager. |
        ///| UserNotEnabled | This user is not enabled yet. |.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestPlaceOrder()
        {
            // Parameters for the API call
            Standard.Models.PlaceOrderRequest body = ApiHelper.JsonDeserialize<Standard.Models.PlaceOrderRequest>("{\r\n  \"RequestId\": \"{{$guid}}\",\r\n  \"AccountId\": 0,\r\n  \"Products\": [\r\n    {\r\n      \"ProductId\": 0,\r\n      \"Quantity\": 1,\r\n      \"Value\": 1\r\n    }\r\n  ]\r\n}");

            // Perform API call
            string result = null;
            try
            {
                result = await this.controller.PlaceOrderAsync(body);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.AreEqual("\"71ac2817-e51a-438a-bb7b-5ffdda23603c\"", TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody), "Response body should match exactly (string literal match)");
        }

        /// <summary>
        /// User have to include valid credentials in the Authorization header of each GET request. The GET/api/integration/v1.0/orders/checkout/{requestId} endpoint lets you get the order. A successful GET request returns order information with cards requested in the response body with the information for each product requested.
        ///
        ///### Example response:
        ///
        ///**Order Status Explanation:**
        ///
        ///- **Created:**
        ///    
        ///    - The initial status is assigned to an order upon its creation in the system. At this stage, the order is ready for processing.
        ///        
        ///- **Processing:**
        ///    
        ///    - Indicates that the system has accepted the order, and it is in progress.
        ///        
        ///- **Pending:**
        ///    
        ///    - The order is awaiting further action or approval before it can proceed to the next stage. This status is often temporary.
        ///        
        ///- **Succeeded:**
        ///    
        ///    - Signifies that the order has been successfully completed, and the requested operation or transaction has been carried out without any issues.
        ///        
        ///- **Failed:**
        ///    
        ///    - Indicates that the order encountered an error or faced an obstacle during processing, preventing successful completion.
        ///        
        ///- **PartialFailed:**
        ///    
        ///    - A hybrid status indicates that certain aspects of the order were successful, while others encountered issues or failures. This status provides a nuanced view of the order's overall outcome.
        ///        
        ///
        ///**Remarks:**
        ///
        ///- _If you are getting the status Pending, this means that the order is currently being processed, wait until you get another status._
        ///    
        ///
        ///``` json
        ///{
        ///    "orderId": 1,
        ///    "requestId": "97018cf7-98f5-40fc-a142-711d43a72e17",
        ///    "items": [
        ///        {
        ///            "brandCode": "string",
        ///            "productId": 1,
        ///            "productFaceValue": 1,
        ///            "quantity": 1,
        ///            "pictureUrl": "string",
        ///            "countryCode": "string",
        ///            "currencyCode": "string",
        ///            "status" : "string"
        ///            "cards": [
        ///                {
        ///                    "id": 1,
        ///                    "serialNumber": "string",
        ///                    "cardCode": "string",
        ///                    "pin": "string",
        ///                    "expirationDate": "string",
        ///                    "status": "string"
        ///                }
        ///            ]
        ///        }
        ///    ],
        ///    "status": "string",
        ///    "createdDate": "2020-09-25T14:04:50.00000",
        ///    "total": 1,
        ///    "errorMessage": "string"
        ///    "orderType": "Sandbox",
        ///    "currency": "USD"
        ///}
        ///
        /// ```
        ///
        ///### Response Remarks:
        ///
        ///**Response Remarks:**
        ///
        ///- **Purchase Outcome:**
        ///    
        ///    - In some instances, not all cards may be successfully purchased. In such cases, a failure message is returned specifically for the cards that were not acquired.
        ///        
        ///- **Order Status, Order Items Status Indicators:**
        ///    
        ///    - The order can have the following statuses:
        ///        
        ///        - **Succeeded:** Successfully processed.
        ///            
        ///        - **Created:** Order is created but processing has not commenced.
        ///            
        ///        - **Failed:** The order encountered an issue, and no cards were purchased.
        ///            
        ///        - **PartialFailed:** Some cards were bought, but others encountered issues.
        ///            
        ///- **Total Amount Charged:**
        ///    
        ///    - The "Total" field in the response indicates the total amount of money charged from the account in its respective currency.
        ///        
        ///
        ///**Card Status Explanation:**
        ///
        ///- **Paused:**
        ///    
        ///    - Cards in a paused status are awaiting retrieval from different channels.
        ///        
        ///- **Sold:**
        ///    
        ///    - Cards that have been successfully purchased.
        ///        
        ///- **Failed:**
        ///    
        ///    - Cards that encountered failures during the purchase attempt
        ///        
        ///    - and no charges will be applied to them.
        ///        
        ///- **Created:**
        ///    
        ///    - Indicates that order processing for the respective card has not started yet.
        ///        
        ///- **Processing:**
        ///    
        ///    - Denotes that the system is currently processing the card within the order..
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestGetOrder()
        {
            // Parameters for the API call
            string id = "fb855c6a-f8de-4a5a-a87b-d2d7a7a4d049";

            // Perform API call
            Standard.Models.GetOrder result = null;
            try
            {
                result = await this.controller.GetOrderAsync(id);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "{\r\n  \"orderId\": 6779294,\r\n  \"requestId\": \"71ac2817-e51a-438a-bb7b-5ffdda23603c\",\r\n  \"items\": [\r\n    {\r\n      \"brandCode\": null,\r\n      \"productId\": 439685,\r\n      \"productFaceValue\": 10,\r\n      \"quantity\": 2,\r\n      \"pictureUrl\": null,\r\n      \"countryCode\": \"BH\",\r\n      \"currencyCode\": \"USD\",\r\n      \"cards\": [\r\n        {\r\n          \"id\": 24153984,\r\n          \"serialNumber\": \"4005c08f-1aeb-4527-8eb0-ccd1c745f467\",\r\n          \"cardCode\": \"cb05747ed9fe497b92c80f10cf8406d7\",\r\n          \"pin\": \"392\",\r\n          \"expirationDate\": \"2025-05-16T04:36:35.3922677\",\r\n          \"status\": \"Sold\"\r\n        },\r\n        {\r\n          \"id\": 24153985,\r\n          \"serialNumber\": \"29744102-1e84-48e0-a440-8b3a5c04b4d2\",\r\n          \"cardCode\": \"7dffe979e54c49f692e7894eb91407d1\",\r\n          \"pin\": \"392\",\r\n          \"expirationDate\": \"2025-05-16T04:36:35.3922628\",\r\n          \"status\": \"Sold\"\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"status\": \"Succeeded\",\r\n  \"createdDate\": \"2024-05-16T04:36:35.3673422\",\r\n  \"total\": 18.9401,\r\n  \"errorMessage\": null,\r\n  \"orderType\": \"Sandbox\",\r\n  \"currency\": \"USD\"\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// **Exchange Rate Retrieval Guidelines:**
        ///
        ///- **Credential Inclusion:**
        ///    
        ///    - Ensure the inclusion of valid credentials in the Authorization header for each GET request.
        ///        
        ///- **Endpoint for Exchange Rates:**
        ///    
        ///    - Utilize the GET endpoint to retrieve the list of exchange rates used in the system.
        ///        
        ///- **Query Parameters:**
        ///    
        ///    - If a request is sent without query parameters, the endpoint returns a list of all exchange rates based on USD.
        ///        
        ///- **Base Currency Parameter:**
        ///    
        ///    - If a request includes the baseCurrency parameter, the endpoint will return a list of all exchange rates based on the specified base currency.
        ///        
        ///- **Base and Currency Parameters:**
        ///    
        ///    - If a request contains both baseCurrency and currency parameters, the endpoint will specifically return the exchange rate between the base currency and the specified currency.
        ///        
        ///
        ///### Example response:
        ///
        ///``` json
        ///{
        ///    "baseCurrencyCode": "USD",
        ///    "rates": [
        ///        {
        ///            "value": 0.8714830000,
        ///            "currencyCode": "EUR"
        ///        }
        ///    ]
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestExchangeRates()
        {
            // Parameters for the API call
            string baseCurrency = "AED";
            string currency = "INR";

            // Perform API call
            Standard.Models.ExchangeRates result = null;
            try
            {
                result = await this.controller.ExchangeRatesAsync(baseCurrency, currency);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "{\r\n  \"baseCurrencyCode\": \"AED\",\r\n  \"rates\": [\r\n    {\r\n      \"currencyCode\": \"INR\",\r\n      \"value\": 22.718078\r\n    }\r\n  ]\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// When the the user calls the aforementioned API providing valid dates ( the range containing data ) and a valid username and password in Basic Auth, an Object in the following format will be returned.
        ///
        ///``` json
        ///"startDate": "2022-10-01T00:00:00",
        ///"endDate": "2022-11-20T23:59:59",
        ///"clients": [
        ///        {
        ///            "clientName": "string",
        ///            "transactions": [
        ///                {
        ///                    "orderId": 3824312,
        ///                    "requestId": "string",
        ///                    "productId": 483230,
        ///                    "orderNumber": "string",
        ///                    "orderTotal": {
        ///                        "value": 108.5100,
        ///                        "currencyCode": "USD"
        ///                    },
        ///                    "orderTotalForSelectedProducts": {
        ///                        "value": 108.5100,
        ///                        "currencyCode": "USD"
        ///                    },
        ///                    "currencyCode": null,
        ///                    "orderDate": "2023-11-17T07:57:18.8519922",
        ///                    "transactionId": "3752352 ",
        ///                    "transactionType": "Order",
        ///                    "comment": null,
        ///                    "notes": null,
        ///                    "availableBalance": {
        ///                        "value": 99875.8400,
        ///                        "currencyCode": "USD"
        ///                    },
        ///                    "transactionAmount": {
        ///                        "value": 108.5100,
        ///                        "currencyCode": "USD"
        ///                    },
        ///                    "transactionDate": "2023-11-17T07:57:18.8519922",
        ///                    "orderItems": [
        ///                        {
        ///                            "createdDate": "0001-01-01T00:00:00",
        ///                            "productName": "string",
        ///                            "brandName": "string",
        ///                            "brandCurrencyCode": "EUR",
        ///                            "brandWithDenominationCurrency": "string",
        ///                            "denomination": {
        ///                                "value": 10.0000,
        ///                                "currencyCode": "EUR"
        ///                            },
        ///                            "clientDiscountType": null,
        ///                            "clientDiscountValue": 0.0000,
        ///                            "clientTransactionFeeType": null,
        ///                            "clientTransactionFeeValue": 0.0000,
        ///                            "clientPrice": {
        ///                                "value": 108.5100,
        ///                                "currencyCode": "USD"
        ///                            },
        ///                            "estimatedPrice": {
        ///                                "value": 108.5100,
        ///                                "currencyCode": "USD"
        ///                            },
        ///                            "quantity": 10,
        ///                            "clientTotal": 108.5100,
        ///                            "clientExchangeRate": {
        ///                                "fromCurrency": "EUR",
        ///                                "toCurrency": "USD",
        ///                                "exchangeRate": 0.0
        ///                            }
        ///                        }
        ///                    ],
        ///                    "clientName": null,
        ///                    "orderType": null,
        ///                    "xeroCreditNoteNumber": null
        ///                }
        ///            ]
        ///        }
        ///    ]
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestTransactions()
        {
            // Parameters for the API call
            string startDate = "2022-10-01";
            string endDate = "2022-11-20";

            // Perform API call
            Standard.Models.Transactions result = null;
            try
            {
                result = await this.controller.TransactionsAsync(startDate, endDate);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "{\r\n  \"startDate\": \"2024-05-15T00:00:00\",\r\n  \"endDate\": \"2024-05-16T23:59:59\",\r\n  \"clients\": [\r\n    {\r\n      \"clientName\": \"Bamboo Tech - Testing\",\r\n      \"transactions\": [\r\n        {\r\n          \"orderId\": 6761537,\r\n          \"requestId\": \"97018cf7-98f5-40fc-a142-711d43a72e17\",\r\n          \"productId\": 439685,\r\n          \"orderNumber\": \"O-06761537\",\r\n          \"orderTotal\": {\r\n            \"value\": 9.47,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"orderTotalForSelectedProducts\": {\r\n            \"value\": 9.47,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"currencyCode\": null,\r\n          \"orderDate\": \"2024-05-15T04:29:41.3062431\",\r\n          \"transactionId\": \"6677709 \",\r\n          \"transactionType\": \"Order\",\r\n          \"comment\": null,\r\n          \"notes\": null,\r\n          \"availableBalance\": {\r\n            \"value\": 100828.4848,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"transactionAmount\": {\r\n            \"value\": 9.47,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"transactionDate\": \"2024-05-15T04:29:41.3062431\",\r\n          \"orderItems\": [\r\n            {\r\n              \"createdDate\": \"0001-01-01T00:00:00\",\r\n              \"productName\": \"Steam 10 USD BH\",\r\n              \"brandName\": \"Steam Bahrain\",\r\n              \"brandCurrencyCode\": \"USD\",\r\n              \"brandWithDenominationCurrency\": \"Steam Bahrain USD\",\r\n              \"denomination\": {\r\n                \"value\": 10,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"clientDiscountType\": \"Percentage\",\r\n              \"clientDiscountValue\": 5.2995,\r\n              \"clientTransactionFeeType\": \"Fixed\",\r\n              \"clientTransactionFeeValue\": 0,\r\n              \"clientPrice\": {\r\n                \"value\": 9.47,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"estimatedPrice\": {\r\n                \"value\": 9.47,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"quantity\": 1,\r\n              \"clientTotal\": 9.47,\r\n              \"clientExchangeRate\": {\r\n                \"fromCurrency\": \"USD\",\r\n                \"toCurrency\": \"USD\",\r\n                \"exchangeRate\": 0\r\n              }\r\n            }\r\n          ],\r\n          \"clientName\": null,\r\n          \"orderType\": null,\r\n          \"xeroCreditNoteNumber\": null\r\n        },\r\n        {\r\n          \"orderId\": 6779294,\r\n          \"requestId\": \"71ac2817-e51a-438a-bb7b-5ffdda23603c\",\r\n          \"productId\": 439685,\r\n          \"orderNumber\": \"O-06779294\",\r\n          \"orderTotal\": {\r\n            \"value\": 18.9401,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"orderTotalForSelectedProducts\": {\r\n            \"value\": 18.94,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"currencyCode\": null,\r\n          \"orderDate\": \"2024-05-16T04:36:35.4268366\",\r\n          \"transactionId\": \"6695293 \",\r\n          \"transactionType\": \"Order\",\r\n          \"comment\": null,\r\n          \"notes\": null,\r\n          \"availableBalance\": {\r\n            \"value\": 100809.5447,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"transactionAmount\": {\r\n            \"value\": 18.9401,\r\n            \"currencyCode\": \"USD\"\r\n          },\r\n          \"transactionDate\": \"2024-05-16T04:36:35.4268366\",\r\n          \"orderItems\": [\r\n            {\r\n              \"createdDate\": \"0001-01-01T00:00:00\",\r\n              \"productName\": \"Steam 10 USD BH\",\r\n              \"brandName\": \"Steam Bahrain\",\r\n              \"brandCurrencyCode\": \"USD\",\r\n              \"brandWithDenominationCurrency\": \"Steam Bahrain USD\",\r\n              \"denomination\": {\r\n                \"value\": 10,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"clientDiscountType\": \"Percentage\",\r\n              \"clientDiscountValue\": 5.2995,\r\n              \"clientTransactionFeeType\": \"Fixed\",\r\n              \"clientTransactionFeeValue\": 0,\r\n              \"clientPrice\": {\r\n                \"value\": 18.9401,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"estimatedPrice\": {\r\n                \"value\": 18.94,\r\n                \"currencyCode\": \"USD\"\r\n              },\r\n              \"quantity\": 2,\r\n              \"clientTotal\": 18.94,\r\n              \"clientExchangeRate\": {\r\n                \"fromCurrency\": \"USD\",\r\n                \"toCurrency\": \"USD\",\r\n                \"exchangeRate\": 0\r\n              }\r\n            }\r\n          ],\r\n          \"clientName\": null,\r\n          \"orderType\": null,\r\n          \"xeroCreditNoteNumber\": null\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// - To access the orders history, you must include valid credentials in the Authorization header of each GET request.
        ///    
        ///- Utilize the GET/api/integration/v1.0/orders endpoint.
        ///    
        ///- Upon successful execution of the GET request, the response body will contain the requested orders information, with details for each order included.
        ///    
        ///
        ///``` json
        ///[
        ///    {
        ///        "clientName": "string",
        ///        "orderNumber": "string",
        ///        "orderDate": "2022-11-17T00:00:00",
        ///        "clientReferenceNumber": "string",
        ///        "channel": "string",
        ///        "productName": "string",
        ///        "denomination": "EUR 10",
        ///        "quantity": 10,
        ///        "unitPrice": 10.8510,
        ///        "subTotal": 108.5100,
        ///        "total": 108.5100,
        ///        "accountCurrency": "USD",
        ///        "status": "Succeeded"
        ///    },
        ///]
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestOrders()
        {
            // Parameters for the API call
            string startDate = "2022-10-01";
            string endDate = "2022-11-20";

            // Perform API call
            List<Standard.Models.Order> result = null;
            try
            {
                result = await this.controller.OrdersAsync(startDate, endDate);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "[\r\n  {\r\n    \"clientName\": \"Bamboo Tech - Testing\",\r\n    \"orderNumber\": \"O-06761537\",\r\n    \"orderDate\": \"2024-05-15T00:00:00\",\r\n    \"clientReferenceNumber\": \"97018cf7-98f5-40fc-a142-711d43a72e17\",\r\n    \"channel\": \"Sandbox\",\r\n    \"productName\": \"Steam 10 USD BH\",\r\n    \"denomination\": \"USD 10\",\r\n    \"quantity\": 1,\r\n    \"unitPrice\": 9.47,\r\n    \"subTotal\": 9.47,\r\n    \"total\": 9.47,\r\n    \"accountCurrency\": \"USD\",\r\n    \"status\": \"Succeeded\"\r\n  },\r\n  {\r\n    \"clientName\": \"Bamboo Tech - Testing\",\r\n    \"orderNumber\": \"O-06779294\",\r\n    \"orderDate\": \"2024-05-16T00:00:00\",\r\n    \"clientReferenceNumber\": \"71ac2817-e51a-438a-bb7b-5ffdda23603c\",\r\n    \"channel\": \"Sandbox\",\r\n    \"productName\": \"Steam 10 USD BH\",\r\n    \"denomination\": \"USD 10\",\r\n    \"quantity\": 2,\r\n    \"unitPrice\": 9.47,\r\n    \"subTotal\": 18.94,\r\n    \"total\": 18.9401,\r\n    \"accountCurrency\": \"USD\",\r\n    \"status\": \"Succeeded\"\r\n  }\r\n]",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// This API enables you to configure the notification URL in your system. To enhance security, you have the option to set a secretKey and validate it on your end. Please note that setting the notification URL to empty will disable this feature.
        ///
        ///#### **Notification Callback Response**
        ///
        ///When an order reaches the "complete" status (whether it's Succeeded, Failed, or PartialFailed), our API will initiate a POST request to your configured endpoint. The request will include the following parameters. After receiving this request, you will need to fetch the order details using the Get Order endpoint.
        ///
        ///``` json
        ///{
        ///  "orderId": 12345 ,
        ///  "status": "succeeded",
        ///  "totalCards": 10,
        ///  "createdOn": "datetime order create date", 
        ///  "completedOn": "datetime order completed date",
        ///  "secretKey": "Merchent configured secret key",
        ///  "requestId": "GUID Merchent Identifier"
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestNotification()
        {
            // Parameters for the API call
            Standard.Models.NotificationRequest body = ApiHelper.JsonDeserialize<Standard.Models.NotificationRequest>("{\r\n  \"notificationUrl\": \"merchant end point callback url\",\r\n  \"notificationUrlSandbox\": \"merchant sandbox end point callback url\",\r\n  \"secretKey\": \"merchant end point verification secret key\"\r\n}");

            // Perform API call
            Standard.Models.Notification result = null;
            try
            {
                result = await this.controller.NotificationAsync(body);
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "{\r\n  \"notificationUrl\": \"https://yoururl/order/notification\",\r\n  \"notificationUrlSandbox\": \"https://yoururl/order/notification\",\r\n  \"secretKey\": \"76d247c4-015d-470e-9ff6-5c1c873d1e0d\"\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// This API returns the configured notification URL in your system along with the associated secret key, which will be validated on your side.
        ///
        ///### Example response:
        ///
        ///``` json
        ///{
        ///    "notificationUrl": "your endpoint callback url",
        ///    "notificationUrlSandbox": "merchant sandbox end point callback url",
        ///    "secretKey": "your endpoint verification secret key",
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestNotification1()
        {
            // Perform API call
            Standard.Models.Notification result = null;
            try
            {
                result = await this.controller.Notification1Async();
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");

            // Test headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", null);
            headers.Add("Date", null);
            headers.Add("Content-Length", null);
            headers.Add("Connection", null);
            headers.Add("Set-Cookie", null);
            headers.Add("Server", null);
            headers.Add("api-supported-versions", null);

            Assert.IsTrue(
                    TestHelper.AreHeadersProperSubsetOf (
                    headers,
                    HttpCallBack.Response.Headers),
                    "Headers should match");

            // Test whether the captured response is as we expected
            Assert.IsNotNull(result, "Result should exist");
            Assert.IsTrue(
                    TestHelper.IsProperSubsetOf(
                    "{\r\n  \"notificationUrl\": \"https://yoururl/order/notification\",\r\n  \"notificationUrlSandbox\": \"https://yoururl/order/notification\",\r\n  \"secretKey\": \"76d247c4-015d-470e-9ff6-5c1c873d1e0d\"\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }
    }
}