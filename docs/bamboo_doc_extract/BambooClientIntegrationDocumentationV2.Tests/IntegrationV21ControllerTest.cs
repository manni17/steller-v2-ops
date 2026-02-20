// <copyright file="IntegrationV21ControllerTest.cs" company="APIMatic">
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
    /// IntegrationV21ControllerTest.
    /// </summary>
    [TestFixture]
    public class IntegrationV21ControllerTest : ControllerTestBase
    {
        /// <summary>
        /// Controller instance (for all tests).
        /// </summary>
        private IntegrationV21Controller controller;

        /// <summary>
        /// Setup test class.
        /// </summary>
        [OneTimeSetUp]
        public void SetUpDerived()
        {
            this.controller = this.Client.IntegrationV21Controller;
        }

        /// <summary>
        /// ### GET /api/Integration/v2.0/brands
        ///
        ///This endpoint retrieves a list of brands with optional filtering parameters.
        ///
        ///#### Headers
        ///
        ///- Content-Type: application/json
        ///    
        ///
        ///#### Request Parameters
        ///
        ///- pagesize (optional): The number of items per page.
        ///    
        ///- pageIndex (optional): The index of the page.
        ///    
        ///- currencyCode (optional): Filter by currency code.
        ///    
        ///- countryCode (optional): Filter by country code.
        ///    
        ///- name (optional): Filter by brand name.
        ///    
        ///- sku (optional): Filter by stock keeping unit (SKU).
        ///    
        ///- categoryId (optional): Filter by category ID.
        ///    
        ///- modifiedDate (optional): Filter by modified date.
        ///    
        ///
        ///#### Response
        ///
        ///The response returns a JSON object with the following fields:
        ///
        ///- pageIndex: The index of the returned page.
        ///    
        ///- pageSize: The number of items per page.
        ///    
        ///- count: The total count of items.
        ///    
        ///- items: An array of brand objects with the following properties:
        ///    
        ///    - id: The unique identifier of the brand.
        ///        
        ///    - name: The name of the brand.
        ///        
        ///    - sku: The stock keeping unit (SKU) of the brand.
        ///        
        ///    - countryCode: The country code associated with the brand.
        ///        
        ///    - currencyCode: The currency code associated with the brand.
        ///        
        ///    - description: The description of the brand.
        ///        
        ///    - disclaimer: The disclaimer associated with the brand.
        ///        
        ///    - redemptionInstructions: Instructions for redemption.
        ///        
        ///    - terms: Terms and conditions for the brand.
        ///        
        ///    - status: The status of the brand.
        ///        
        ///    - logoUrl: The URL of the brand's logo.
        ///        
        ///    - modifiedDate: The date of the last modification.
        ///        
        ///    - faceValues: An array of face value objects with min and max values.
        ///        
        ///        - fixed face value > when min and max are equal =>
        ///            
        ///        - range face value > when Min < Max
        ///            
        ///        - steps also as 1 (integer)
        ///            
        ///        - one brand can have multiple face values range and fixed
        ///            
        ///    - categories: An array of category IDs associated with the brand.
        ///        
        ///
        ///Example Response:
        ///
        ///``` json
        ///{
        ///  "pageIndex": 0,
        ///  "pageSize": 0,
        ///  "count": 0,
        ///  "items": [
        ///    {
        ///      "id": 0,
        ///      "name": "",
        ///      "sku": "",
        ///      "countryCode": "",
        ///      "currencyCode": "",
        ///      "description": "",
        ///      "disclaimer": null,
        ///      "redemptionInstructions": "",
        ///      "terms": "",
        ///      "status": "",
        ///      "logoUrl": "",
        ///      "modifiedDate": "",
        ///      "faceValues": [
        ///        {
        ///          "min": 1,
        ///          "max": 500
        ///        },
        ///        {
        ///          "min": 1000,
        ///          "max": 1000
        ///        },
        ///        {
        ///          "min": 2000,
        ///          "max": 5000
        ///        }
        ///      ],
        ///      "categories": [0]
        ///    }
        ///  ]
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestBrands()
        {
            // Parameters for the API call
            int pagesize = 10;
            int pageIndex = 0;
            string currencyCode = string.Empty;
            string countryCode = string.Empty;
            string name = string.Empty;
            string sku = string.Empty;
            string categoryId = string.Empty;
            string modifiedDate = "2023-10-02";

            // Perform API call
            Standard.Models.Brands result = null;
            try
            {
                result = await this.controller.BrandsAsync(pagesize, pageIndex, currencyCode, countryCode, name, sku, categoryId, modifiedDate);
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
                    "{\r\n  \"pageIndex\": 0,\r\n  \"pageSize\": 10,\r\n  \"count\": 2,\r\n  \"items\": [\r\n    {\r\n      \"id\": 2118,\r\n      \"name\": \"Test Brand UAE\",\r\n      \"sku\": \"TBRAND-AE\",\r\n      \"countryCode\": \"AE\",\r\n      \"currencyCode\": \"AED\",\r\n      \"description\": \"test description for Brand UAE\",\r\n      \"disclaimer\": null,\r\n      \"redemptionInstructions\": \"Test redemption instructions for Brand UAE\",\r\n      \"terms\": \"Test Terms and conditions for Brand UAE\",\r\n      \"status\": \"Live\",\r\n      \"logoUrl\": \"https://bamboo-assets.s3.amazonaws.com/app-images/brand-images/2118/card-picture\",\r\n      \"modifiedDate\": \"2024-07-03T06:27:00.1331765\",\r\n      \"faceValues\": [\r\n        {\r\n          \"min\": 1,\r\n          \"max\": 1\r\n        },\r\n        {\r\n          \"min\": 10,\r\n          \"max\": 10\r\n        },\r\n        {\r\n          \"min\": 20,\r\n          \"max\": 20\r\n        }\r\n      ],\r\n      \"categories\": [\r\n        6\r\n      ]\r\n    },\r\n    {\r\n      \"id\": 6132,\r\n      \"name\": \"Test Brand Bahrain\",\r\n      \"sku\": \"TBRAND1-AE\",\r\n      \"countryCode\": \"BH\",\r\n      \"currencyCode\": \"BHD\",\r\n      \"description\": \"Test description\",\r\n      \"disclaimer\": null,\r\n      \"redemptionInstructions\": \"Test redemption instructions\",\r\n      \"terms\": \"Test terms and conditions \",\r\n      \"status\": \"TemporaryInactive\",\r\n      \"logoUrl\": \"\",\r\n      \"modifiedDate\": \"2024-10-16T16:29:48.3522299\",\r\n      \"faceValues\": [],\r\n      \"categories\": []\r\n    }\r\n  ]\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// ### Get Stock Information
        ///
        ///This endpoint retrieves the stock information for a specific item based on the provided SKU and face value.
        ///
        ///#### Request Parameters
        ///
        ///- `sku` (path) - The Stock Keeping Unit (SKU) of the item.
        ///    
        ///- `facevalue` (path) - The face value of the item.
        ///    
        ///
        ///#### Response
        ///
        ///- `faceValue` (number) - The face value of the item.
        ///    
        ///- `quantity` (number) - The available quantity of the item.
        ///    
        ///- `isStocked` (boolean) - Indicates if the item is in stock. If the value is `false`, it indicates that the item is unlimited.
        ///    
        ///- `price` (number) - The price of the item.
        ///    
        ///- `currencyCode` (string) - The Iso-3 currency code for the price.
        ///    
        ///
        ///#### Example Response
        ///
        ///``` json
        ///{
        ///    "faceValue": 0,
        ///    "quantity": 0,
        ///    "isStocked": true,
        ///    "price": 0,
        ///    "currencyCode": ""
        ///}
        ///
        /// ```
        ///
        ///``` json
        ///{
        ///    "type": "object",
        ///    "properties": {
        ///        "faceValue": {
        ///            "type": "number"
        ///        },
        ///        "quantity": {
        ///            "type": "number"
        ///        },
        ///        "isStocked": {
        ///            "type": "boolean"
        ///        },
        ///        "price": {
        ///            "type": "number"
        ///        },
        ///        "currencyCode": {
        ///            "type": "string"
        ///        }
        ///    }
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestStock()
        {
            // Parameters for the API call
            string sku = "TBRAND-AE";
            int facevalue = 10;

            // Perform API call
            Standard.Models.Stock result = null;
            try
            {
                result = await this.controller.StockAsync(sku, facevalue);
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
                    "{\r\n  \"faceValue\": 10,\r\n  \"quantity\": 2,\r\n  \"isStocked\": true,\r\n  \"price\": 20,\r\n  \"currencyCode\": \"BHD\"\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// This endpoint makes an HTTP GET request to retrieve a list of categories from the integration API. The response is in JSON format with a status code of 200.
        ///
        ///### Response
        ///
        ///The response will be a JSON array containing objects representing the categories. Each category object will have the following properties:
        ///
        ///- `id` (number): The unique identifier for the category.
        ///    
        ///- `name` (string): The name of the category.
        ///    
        ///- `description` (string): The description of the category.
        ///    
        ///
        ///Example response:
        ///
        ///``` json
        ///[
        ///    {
        ///        "id": 0,
        ///        "name": "",
        ///        "description": ""
        ///    }
        ///]
        ///
        /// ```
        ///
        ///``` json
        ///{
        ///  "type": "array",
        ///  "items": {
        ///    "type": "object",
        ///    "properties": {
        ///      "id": {
        ///        "type": "number"
        ///      },
        ///      "name": {
        ///        "type": "string"
        ///      },
        ///      "description": {
        ///        "type": "string"
        ///      }
        ///    }
        ///  }
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestCategories()
        {
            // Parameters for the API call
            string cacheControl = "public";

            // Perform API call
            List<Standard.Models.Category3> result = null;
            try
            {
                result = await this.controller.CategoriesAsync(cacheControl);
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
                    "[\r\n  {\r\n    \"id\": 1,\r\n    \"name\": \"Entertainment\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 2,\r\n    \"name\": \"Gaming\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 3,\r\n    \"name\": \"Prepaid Cards\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 4,\r\n    \"name\": \"Fashion & Accessories\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 5,\r\n    \"name\": \"Tools & Home Improvement\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 6,\r\n    \"name\": \"Airlines & Hotels\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 7,\r\n    \"name\": \"Beauty Wellness & Spa\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 9,\r\n    \"name\": \"eCommerce\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 10,\r\n    \"name\": \"Dining & Restaurants\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 11,\r\n    \"name\": \"Kids Fashion & Toys\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 12,\r\n    \"name\": \"Travel & Leisure\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 13,\r\n    \"name\": \"Home & Garden\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 14,\r\n    \"name\": \"Learning\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 15,\r\n    \"name\": \"Sports & Lifestyle\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 16,\r\n    \"name\": \"Electronics\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 17,\r\n    \"name\": \"Crypto\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 18,\r\n    \"name\": \"Supermarkets & Hypermarkets\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 19,\r\n    \"name\": \"Bookstore\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 20,\r\n    \"name\": \"Telecommunication & Internet\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 21,\r\n    \"name\": \"Department Stores\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 22,\r\n    \"name\": \"Mobile Application\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 23,\r\n    \"name\": \"Shopping Mall\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 24,\r\n    \"name\": \"Gas Station\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 25,\r\n    \"name\": \"Drugstore\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 26,\r\n    \"name\": \"Pet Shop & Pet Care\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 27,\r\n    \"name\": \"Charity\",\r\n    \"description\": \"\"\r\n  },\r\n  {\r\n    \"id\": 30,\r\n    \"name\": \"Petroleum\",\r\n    \"description\": \"\"\r\n  }\r\n]",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }

        /// <summary>
        /// The endpoint retrieves a list of catalog countries through an HTTP GET request to {{host}}/api/integration/v2.0/catalog-countries.
        ///
        ///The response returned is a JSON array with the following schema:
        ///
        ///``` json
        ///[
        ///  {
        ///    "code": "",
        ///    "name": "",
        ///    "defaultCurrencyCode": "",
        ///    "alpha3Code": ""
        ///  }
        ///]
        ///
        /// ```
        ///
        ///This JSON schema represents the structure of the response where each object in the array contains the code, name, defaultCurrencyCode, and alpha3Code for a catalog country.
        ///
        ///The response to the request will have a status code of 200 and a content type of application/json. The response body will contain an array of objects, where each object represents a catalog country. Each country object includes the following properties:
        ///
        ///- code: The code of the country
        ///    
        ///- name: The name of the country
        ///    
        ///- defaultCurrencyCode: The default currency code of the country
        ///    
        ///- alpha3Code: The alpha-3 code of the country.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestCountries()
        {
            // Perform API call
            try
            {
                await this.controller.CountriesAsync();
            }
            catch (ApiException)
            {
            }

            // Test response code
            Assert.AreEqual(200, HttpCallBack.Response.StatusCode, "Status should be 200");
        }

        /// <summary>
        /// ### Checkout Order
        ///
        ///This endpoint allows you to place an order by sending a POST request to the specified URL.
        ///
        ///#### Request Body
        ///
        ///The request body should be in JSON format and include the following parameters:
        ///
        ///- `RequestId` (string) - A unique identifier for the request.
        ///    
        ///- `AccountId` (number) - The account ID associated with the order.
        ///    
        ///- `Products` (array) - An array of objects containing details about the products being ordered, including `Sku` (string), `Quantity` (number), and `Value` (number).
        ///    
        ///
        ///#### Response
        ///
        ///Upon successful execution, the endpoint returns a JSON response with the following fields:
        ///
        ///- `requestId` (string) - The unique identifier for the request.
        ///    
        ///- `orderNumber` (string) - The order number generated for the placed order.
        ///    
        ///- `status` (string) - The status of the order.
        ///    
        ///- `currencyCode` (string) - The currency code used for the transaction.
        ///    
        ///- `totalCards` (number) - The total number of cards associated with the order.
        ///    
        ///
        ///#### Example Request
        ///
        ///``` json
        ///{
        ///  "RequestId": "{{$guid}}",
        ///  "AccountId": 307,
        ///  "Products": [
        ///    {
        ///      "Sku": "TBRAND-AE",
        ///      "Quantity": 1,
        ///      "Value": 10
        ///    }
        ///  ]
        ///}
        ///
        /// ```
        ///
        ///#### Example Response
        ///
        ///``` json
        ///{
        ///  "requestId": "",
        ///  "orderNumber": "",
        ///  "status": "",
        ///  "currencyCode": "",
        ///  "totalCards": 0
        ///}
        ///
        /// ```.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Test]
        public async Task TestPlaceOrder()
        {
            // Parameters for the API call
            string cacheControl = "public";
            Standard.Models.PlaceOrderRequest1 body = ApiHelper.JsonDeserialize<Standard.Models.PlaceOrderRequest1>("{\r\n  \"RequestId\": \"{{$guid}}\",\r\n  \"AccountId\": 307,\r\n  \"Products\": [\r\n    {\r\n      \"Sku\": \"TBRAND-AE\",\r\n      \"Quantity\": 1,\r\n      \"Value\": 10\r\n    }\r\n  ]\r\n}");

            // Perform API call
            Standard.Models.PlaceOrder result = null;
            try
            {
                result = await this.controller.PlaceOrderAsync(cacheControl, body);
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
                    "{\r\n  \"requestId\": \"efb96235-5ec7-4969-852b-ccea48c8d906\",\r\n  \"orderNumber\": \"O-10531861\",\r\n  \"status\": \"Created\",\r\n  \"currencyCode\": \"USD\",\r\n  \"totalCards\": 1\r\n}",
                    TestHelper.ConvertStreamToString(HttpCallBack.Response.RawBody),
                    false,
                    true,
                    false),
                    "Response body should have matching keys");
        }
    }
}