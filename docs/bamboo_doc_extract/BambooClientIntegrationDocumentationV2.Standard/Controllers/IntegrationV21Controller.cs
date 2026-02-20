// <copyright file="IntegrationV21Controller.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using APIMatic.Core;
using APIMatic.Core.Types;
using APIMatic.Core.Utilities;
using APIMatic.Core.Utilities.Date.Xml;
using BambooClientIntegrationDocumentationV2.Standard;
using BambooClientIntegrationDocumentationV2.Standard.Http.Client;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;
using Newtonsoft.Json.Converters;
using System.Net.Http;

namespace BambooClientIntegrationDocumentationV2.Standard.Controllers
{
    /// <summary>
    /// IntegrationV21Controller.
    /// </summary>
    public class IntegrationV21Controller : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationV21Controller"/> class.
        /// </summary>
        internal IntegrationV21Controller(GlobalConfiguration globalConfiguration) : base(globalConfiguration) { }

        /// <summary>
        /// <![CDATA[
        /// ### GET /api/Integration/v2.0/brands.
        /// This endpoint retrieves a list of brands with optional filtering parameters.
        /// #### Headers.
        /// - Content-Type: application/json.
        ///     .
        /// #### Request Parameters.
        /// - pagesize (optional): The number of items per page.
        ///     .
        /// - pageIndex (optional): The index of the page.
        ///     .
        /// - currencyCode (optional): Filter by currency code.
        ///     .
        /// - countryCode (optional): Filter by country code.
        ///     .
        /// - name (optional): Filter by brand name.
        ///     .
        /// - sku (optional): Filter by stock keeping unit (SKU).
        ///     .
        /// - categoryId (optional): Filter by category ID.
        ///     .
        /// - modifiedDate (optional): Filter by modified date.
        ///     .
        /// #### Response.
        /// The response returns a JSON object with the following fields:.
        /// - pageIndex: The index of the returned page.
        ///     .
        /// - pageSize: The number of items per page.
        ///     .
        /// - count: The total count of items.
        ///     .
        /// - items: An array of brand objects with the following properties:.
        ///     .
        ///     - id: The unique identifier of the brand.
        ///         .
        ///     - name: The name of the brand.
        ///         .
        ///     - sku: The stock keeping unit (SKU) of the brand.
        ///         .
        ///     - countryCode: The country code associated with the brand.
        ///         .
        ///     - currencyCode: The currency code associated with the brand.
        ///         .
        ///     - description: The description of the brand.
        ///         .
        ///     - disclaimer: The disclaimer associated with the brand.
        ///         .
        ///     - redemptionInstructions: Instructions for redemption.
        ///         .
        ///     - terms: Terms and conditions for the brand.
        ///         .
        ///     - status: The status of the brand.
        ///         .
        ///     - logoUrl: The URL of the brand's logo.
        ///         .
        ///     - modifiedDate: The date of the last modification.
        ///         .
        ///     - faceValues: An array of face value objects with min and max values.
        ///         .
        ///         - fixed face value > when min and max are equal =>.
        ///             .
        ///         - range face value > when Min < Max.
        ///             .
        ///         - steps also as 1 (integer).
        ///             .
        ///         - one brand can have multiple face values range and fixed.
        ///             .
        ///     - categories: An array of category IDs associated with the brand.
        ///         .
        /// Example Response:.
        /// ``` json.
        /// {.
        ///   "pageIndex": 0,.
        ///   "pageSize": 0,.
        ///   "count": 0,.
        ///   "items": [.
        ///     {.
        ///       "id": 0,.
        ///       "name": "",.
        ///       "sku": "",.
        ///       "countryCode": "",.
        ///       "currencyCode": "",.
        ///       "description": "",.
        ///       "disclaimer": null,.
        ///       "redemptionInstructions": "",.
        ///       "terms": "",.
        ///       "status": "",.
        ///       "logoUrl": "",.
        ///       "modifiedDate": "",.
        ///       "faceValues": [.
        ///         {.
        ///           "min": 1,.
        ///           "max": 500.
        ///         },.
        ///         {.
        ///           "min": 1000,.
        ///           "max": 1000.
        ///         },.
        ///         {.
        ///           "min": 2000,.
        ///           "max": 5000.
        ///         }.
        ///       ],.
        ///       "categories": [0].
        ///     }.
        ///   ].
        /// }.
        ///  ```.
        /// ]]>
        /// </summary>
        /// <param name="pagesize">Required parameter: Example: .</param>
        /// <param name="pageIndex">Required parameter: Example: .</param>
        /// <param name="currencyCode">Required parameter: Iso 3 currency code.</param>
        /// <param name="countryCode">Required parameter: Iso 2 country code.</param>
        /// <param name="name">Required parameter: Contain in brand name.</param>
        /// <param name="sku">Required parameter: brand Sku.</param>
        /// <param name="categoryId">Required parameter: category Id.</param>
        /// <param name="modifiedDate">Required parameter: Modified After.</param>
        /// <returns>Returns the Models.Brands response from the API call.</returns>
        public Models.Brands Brands(
                int pagesize,
                int pageIndex,
                string currencyCode,
                string countryCode,
                string name,
                string sku,
                string categoryId,
                string modifiedDate)
            => CoreHelper.RunTask(BrandsAsync(pagesize, pageIndex, currencyCode, countryCode, name, sku, categoryId, modifiedDate));

        /// <summary>
        /// <![CDATA[
        /// ### GET /api/Integration/v2.0/brands.
        /// This endpoint retrieves a list of brands with optional filtering parameters.
        /// #### Headers.
        /// - Content-Type: application/json.
        ///     .
        /// #### Request Parameters.
        /// - pagesize (optional): The number of items per page.
        ///     .
        /// - pageIndex (optional): The index of the page.
        ///     .
        /// - currencyCode (optional): Filter by currency code.
        ///     .
        /// - countryCode (optional): Filter by country code.
        ///     .
        /// - name (optional): Filter by brand name.
        ///     .
        /// - sku (optional): Filter by stock keeping unit (SKU).
        ///     .
        /// - categoryId (optional): Filter by category ID.
        ///     .
        /// - modifiedDate (optional): Filter by modified date.
        ///     .
        /// #### Response.
        /// The response returns a JSON object with the following fields:.
        /// - pageIndex: The index of the returned page.
        ///     .
        /// - pageSize: The number of items per page.
        ///     .
        /// - count: The total count of items.
        ///     .
        /// - items: An array of brand objects with the following properties:.
        ///     .
        ///     - id: The unique identifier of the brand.
        ///         .
        ///     - name: The name of the brand.
        ///         .
        ///     - sku: The stock keeping unit (SKU) of the brand.
        ///         .
        ///     - countryCode: The country code associated with the brand.
        ///         .
        ///     - currencyCode: The currency code associated with the brand.
        ///         .
        ///     - description: The description of the brand.
        ///         .
        ///     - disclaimer: The disclaimer associated with the brand.
        ///         .
        ///     - redemptionInstructions: Instructions for redemption.
        ///         .
        ///     - terms: Terms and conditions for the brand.
        ///         .
        ///     - status: The status of the brand.
        ///         .
        ///     - logoUrl: The URL of the brand's logo.
        ///         .
        ///     - modifiedDate: The date of the last modification.
        ///         .
        ///     - faceValues: An array of face value objects with min and max values.
        ///         .
        ///         - fixed face value > when min and max are equal =>.
        ///             .
        ///         - range face value > when Min < Max.
        ///             .
        ///         - steps also as 1 (integer).
        ///             .
        ///         - one brand can have multiple face values range and fixed.
        ///             .
        ///     - categories: An array of category IDs associated with the brand.
        ///         .
        /// Example Response:.
        /// ``` json.
        /// {.
        ///   "pageIndex": 0,.
        ///   "pageSize": 0,.
        ///   "count": 0,.
        ///   "items": [.
        ///     {.
        ///       "id": 0,.
        ///       "name": "",.
        ///       "sku": "",.
        ///       "countryCode": "",.
        ///       "currencyCode": "",.
        ///       "description": "",.
        ///       "disclaimer": null,.
        ///       "redemptionInstructions": "",.
        ///       "terms": "",.
        ///       "status": "",.
        ///       "logoUrl": "",.
        ///       "modifiedDate": "",.
        ///       "faceValues": [.
        ///         {.
        ///           "min": 1,.
        ///           "max": 500.
        ///         },.
        ///         {.
        ///           "min": 1000,.
        ///           "max": 1000.
        ///         },.
        ///         {.
        ///           "min": 2000,.
        ///           "max": 5000.
        ///         }.
        ///       ],.
        ///       "categories": [0].
        ///     }.
        ///   ].
        /// }.
        ///  ```.
        /// ]]>
        /// </summary>
        /// <param name="pagesize">Required parameter: Example: .</param>
        /// <param name="pageIndex">Required parameter: Example: .</param>
        /// <param name="currencyCode">Required parameter: Iso 3 currency code.</param>
        /// <param name="countryCode">Required parameter: Iso 2 country code.</param>
        /// <param name="name">Required parameter: Contain in brand name.</param>
        /// <param name="sku">Required parameter: brand Sku.</param>
        /// <param name="categoryId">Required parameter: category Id.</param>
        /// <param name="modifiedDate">Required parameter: Modified After.</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.Brands response from the API call.</returns>
        public async Task<Models.Brands> BrandsAsync(
                int pagesize,
                int pageIndex,
                string currencyCode,
                string countryCode,
                string name,
                string sku,
                string categoryId,
                string modifiedDate,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.Brands>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/Integration/v2.0/brands")
                  .WithAuth("basic")
                  .Parameters(_parameters => _parameters
                      .Query(_query => _query.Setup("pagesize", pagesize))
                      .Query(_query => _query.Setup("pageIndex", pageIndex))
                      .Query(_query => _query.Setup("currencyCode", currencyCode))
                      .Query(_query => _query.Setup("countryCode", countryCode))
                      .Query(_query => _query.Setup("name", name))
                      .Query(_query => _query.Setup("sku", sku))
                      .Query(_query => _query.Setup("categoryId", categoryId))
                      .Query(_query => _query.Setup("modifiedDate", modifiedDate))))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// ### Get Stock Information.
        /// This endpoint retrieves the stock information for a specific item based on the provided SKU and face value.
        /// #### Request Parameters.
        /// - `sku` (path) - The Stock Keeping Unit (SKU) of the item.
        ///     .
        /// - `facevalue` (path) - The face value of the item.
        ///     .
        /// #### Response.
        /// - `faceValue` (number) - The face value of the item.
        ///     .
        /// - `quantity` (number) - The available quantity of the item.
        ///     .
        /// - `isStocked` (boolean) - Indicates if the item is in stock. If the value is `false`, it indicates that the item is unlimited.
        ///     .
        /// - `price` (number) - The price of the item.
        ///     .
        /// - `currencyCode` (string) - The Iso-3 currency code for the price.
        ///     .
        /// #### Example Response.
        /// ``` json.
        /// {.
        ///     "faceValue": 0,.
        ///     "quantity": 0,.
        ///     "isStocked": true,.
        ///     "price": 0,.
        ///     "currencyCode": "".
        /// }.
        ///  ```.
        /// ``` json.
        /// {.
        ///     "type": "object",.
        ///     "properties": {.
        ///         "faceValue": {.
        ///             "type": "number".
        ///         },.
        ///         "quantity": {.
        ///             "type": "number".
        ///         },.
        ///         "isStocked": {.
        ///             "type": "boolean".
        ///         },.
        ///         "price": {.
        ///             "type": "number".
        ///         },.
        ///         "currencyCode": {.
        ///             "type": "string".
        ///         }.
        ///     }.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="sku">Required parameter: unique Identifire.</param>
        /// <param name="facevalue">Required parameter: requested face value.</param>
        /// <returns>Returns the Models.Stock response from the API call.</returns>
        public Models.Stock Stock(
                string sku,
                int facevalue)
            => CoreHelper.RunTask(StockAsync(sku, facevalue));

        /// <summary>
        /// ### Get Stock Information.
        /// This endpoint retrieves the stock information for a specific item based on the provided SKU and face value.
        /// #### Request Parameters.
        /// - `sku` (path) - The Stock Keeping Unit (SKU) of the item.
        ///     .
        /// - `facevalue` (path) - The face value of the item.
        ///     .
        /// #### Response.
        /// - `faceValue` (number) - The face value of the item.
        ///     .
        /// - `quantity` (number) - The available quantity of the item.
        ///     .
        /// - `isStocked` (boolean) - Indicates if the item is in stock. If the value is `false`, it indicates that the item is unlimited.
        ///     .
        /// - `price` (number) - The price of the item.
        ///     .
        /// - `currencyCode` (string) - The Iso-3 currency code for the price.
        ///     .
        /// #### Example Response.
        /// ``` json.
        /// {.
        ///     "faceValue": 0,.
        ///     "quantity": 0,.
        ///     "isStocked": true,.
        ///     "price": 0,.
        ///     "currencyCode": "".
        /// }.
        ///  ```.
        /// ``` json.
        /// {.
        ///     "type": "object",.
        ///     "properties": {.
        ///         "faceValue": {.
        ///             "type": "number".
        ///         },.
        ///         "quantity": {.
        ///             "type": "number".
        ///         },.
        ///         "isStocked": {.
        ///             "type": "boolean".
        ///         },.
        ///         "price": {.
        ///             "type": "number".
        ///         },.
        ///         "currencyCode": {.
        ///             "type": "string".
        ///         }.
        ///     }.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="sku">Required parameter: unique Identifire.</param>
        /// <param name="facevalue">Required parameter: requested face value.</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.Stock response from the API call.</returns>
        public async Task<Models.Stock> StockAsync(
                string sku,
                int facevalue,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.Stock>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/integration/v2.0/getstock/{sku}/{facevalue}")
                  .WithAuth("basic")
                  .Parameters(_parameters => _parameters
                      .Template(_template => _template.Setup("sku", sku))
                      .Template(_template => _template.Setup("facevalue", facevalue))))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// This endpoint makes an HTTP GET request to retrieve a list of categories from the integration API. The response is in JSON format with a status code of 200.
        /// ### Response.
        /// The response will be a JSON array containing objects representing the categories. Each category object will have the following properties:.
        /// - `id` (number): The unique identifier for the category.
        ///     .
        /// - `name` (string): The name of the category.
        ///     .
        /// - `description` (string): The description of the category.
        ///     .
        /// Example response:.
        /// ``` json.
        /// [.
        ///     {.
        ///         "id": 0,.
        ///         "name": "",.
        ///         "description": "".
        ///     }.
        /// ].
        ///  ```.
        /// ``` json.
        /// {.
        ///   "type": "array",.
        ///   "items": {.
        ///     "type": "object",.
        ///     "properties": {.
        ///       "id": {.
        ///         "type": "number".
        ///       },.
        ///       "name": {.
        ///         "type": "string".
        ///       },.
        ///       "description": {.
        ///         "type": "string".
        ///       }.
        ///     }.
        ///   }.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="cacheControl">Required parameter: Example: .</param>
        /// <returns>Returns the List of Models.Category3 response from the API call.</returns>
        public List<Models.Category3> Categories(
                string cacheControl)
            => CoreHelper.RunTask(CategoriesAsync(cacheControl));

        /// <summary>
        /// This endpoint makes an HTTP GET request to retrieve a list of categories from the integration API. The response is in JSON format with a status code of 200.
        /// ### Response.
        /// The response will be a JSON array containing objects representing the categories. Each category object will have the following properties:.
        /// - `id` (number): The unique identifier for the category.
        ///     .
        /// - `name` (string): The name of the category.
        ///     .
        /// - `description` (string): The description of the category.
        ///     .
        /// Example response:.
        /// ``` json.
        /// [.
        ///     {.
        ///         "id": 0,.
        ///         "name": "",.
        ///         "description": "".
        ///     }.
        /// ].
        ///  ```.
        /// ``` json.
        /// {.
        ///   "type": "array",.
        ///   "items": {.
        ///     "type": "object",.
        ///     "properties": {.
        ///       "id": {.
        ///         "type": "number".
        ///       },.
        ///       "name": {.
        ///         "type": "string".
        ///       },.
        ///       "description": {.
        ///         "type": "string".
        ///       }.
        ///     }.
        ///   }.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="cacheControl">Required parameter: Example: .</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the List of Models.Category3 response from the API call.</returns>
        public async Task<List<Models.Category3>> CategoriesAsync(
                string cacheControl,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<List<Models.Category3>>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/integration/v2.0/categories")
                  .WithAuth("basic")
                  .Parameters(_parameters => _parameters
                      .Header(_header => _header.Setup("Cache-Control", cacheControl))))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// The endpoint retrieves a list of catalog countries through an HTTP GET request to {{host}}/api/integration/v2.0/catalog-countries.
        /// The response returned is a JSON array with the following schema:.
        /// ``` json.
        /// [.
        ///   {.
        ///     "code": "",.
        ///     "name": "",.
        ///     "defaultCurrencyCode": "",.
        ///     "alpha3Code": "".
        ///   }.
        /// ].
        ///  ```.
        /// This JSON schema represents the structure of the response where each object in the array contains the code, name, defaultCurrencyCode, and alpha3Code for a catalog country.
        /// The response to the request will have a status code of 200 and a content type of application/json. The response body will contain an array of objects, where each object represents a catalog country. Each country object includes the following properties:.
        /// - code: The code of the country.
        ///     .
        /// - name: The name of the country.
        ///     .
        /// - defaultCurrencyCode: The default currency code of the country.
        ///     .
        /// - alpha3Code: The alpha-3 code of the country.
        /// </summary>
        public void Countries()
            => CoreHelper.RunVoidTask(CountriesAsync());

        /// <summary>
        /// The endpoint retrieves a list of catalog countries through an HTTP GET request to {{host}}/api/integration/v2.0/catalog-countries.
        /// The response returned is a JSON array with the following schema:.
        /// ``` json.
        /// [.
        ///   {.
        ///     "code": "",.
        ///     "name": "",.
        ///     "defaultCurrencyCode": "",.
        ///     "alpha3Code": "".
        ///   }.
        /// ].
        ///  ```.
        /// This JSON schema represents the structure of the response where each object in the array contains the code, name, defaultCurrencyCode, and alpha3Code for a catalog country.
        /// The response to the request will have a status code of 200 and a content type of application/json. The response body will contain an array of objects, where each object represents a catalog country. Each country object includes the following properties:.
        /// - code: The code of the country.
        ///     .
        /// - name: The name of the country.
        ///     .
        /// - defaultCurrencyCode: The default currency code of the country.
        ///     .
        /// - alpha3Code: The alpha-3 code of the country.
        /// </summary>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the void response from the API call.</returns>
        public async Task CountriesAsync(CancellationToken cancellationToken = default)
            => await CreateApiCall<VoidType>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/integration/v2.0/catalog-countries")
                  .WithAuth("basic"))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// ### Checkout Order.
        /// This endpoint allows you to place an order by sending a POST request to the specified URL.
        /// #### Request Body.
        /// The request body should be in JSON format and include the following parameters:.
        /// - `RequestId` (string) - A unique identifier for the request.
        ///     .
        /// - `AccountId` (number) - The account ID associated with the order.
        ///     .
        /// - `Products` (array) - An array of objects containing details about the products being ordered, including `Sku` (string), `Quantity` (number), and `Value` (number).
        ///     .
        /// #### Response.
        /// Upon successful execution, the endpoint returns a JSON response with the following fields:.
        /// - `requestId` (string) - The unique identifier for the request.
        ///     .
        /// - `orderNumber` (string) - The order number generated for the placed order.
        ///     .
        /// - `status` (string) - The status of the order.
        ///     .
        /// - `currencyCode` (string) - The currency code used for the transaction.
        ///     .
        /// - `totalCards` (number) - The total number of cards associated with the order.
        ///     .
        /// #### Example Request.
        /// ``` json.
        /// {.
        ///   "RequestId": "{{$guid}}",.
        ///   "AccountId": 307,.
        ///   "Products": [.
        ///     {.
        ///       "Sku": "TBRAND-AE",.
        ///       "Quantity": 1,.
        ///       "Value": 10.
        ///     }.
        ///   ].
        /// }.
        ///  ```.
        /// #### Example Response.
        /// ``` json.
        /// {.
        ///   "requestId": "",.
        ///   "orderNumber": "",.
        ///   "status": "",.
        ///   "currencyCode": "",.
        ///   "totalCards": 0.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="cacheControl">Required parameter: Example: .</param>
        /// <param name="body">Required parameter: Example: .</param>
        /// <returns>Returns the Models.PlaceOrder response from the API call.</returns>
        public Models.PlaceOrder PlaceOrder(
                string cacheControl,
                Models.PlaceOrderRequest1 body)
            => CoreHelper.RunTask(PlaceOrderAsync(cacheControl, body));

        /// <summary>
        /// ### Checkout Order.
        /// This endpoint allows you to place an order by sending a POST request to the specified URL.
        /// #### Request Body.
        /// The request body should be in JSON format and include the following parameters:.
        /// - `RequestId` (string) - A unique identifier for the request.
        ///     .
        /// - `AccountId` (number) - The account ID associated with the order.
        ///     .
        /// - `Products` (array) - An array of objects containing details about the products being ordered, including `Sku` (string), `Quantity` (number), and `Value` (number).
        ///     .
        /// #### Response.
        /// Upon successful execution, the endpoint returns a JSON response with the following fields:.
        /// - `requestId` (string) - The unique identifier for the request.
        ///     .
        /// - `orderNumber` (string) - The order number generated for the placed order.
        ///     .
        /// - `status` (string) - The status of the order.
        ///     .
        /// - `currencyCode` (string) - The currency code used for the transaction.
        ///     .
        /// - `totalCards` (number) - The total number of cards associated with the order.
        ///     .
        /// #### Example Request.
        /// ``` json.
        /// {.
        ///   "RequestId": "{{$guid}}",.
        ///   "AccountId": 307,.
        ///   "Products": [.
        ///     {.
        ///       "Sku": "TBRAND-AE",.
        ///       "Quantity": 1,.
        ///       "Value": 10.
        ///     }.
        ///   ].
        /// }.
        ///  ```.
        /// #### Example Response.
        /// ``` json.
        /// {.
        ///   "requestId": "",.
        ///   "orderNumber": "",.
        ///   "status": "",.
        ///   "currencyCode": "",.
        ///   "totalCards": 0.
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="cacheControl">Required parameter: Example: .</param>
        /// <param name="body">Required parameter: Example: .</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.PlaceOrder response from the API call.</returns>
        public async Task<Models.PlaceOrder> PlaceOrderAsync(
                string cacheControl,
                Models.PlaceOrderRequest1 body,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.PlaceOrder>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Post, "/integration/v2.0/orders/checkout")
                  .WithAuth("basic")
                  .Parameters(_parameters => _parameters
                      .Body(_bodyParameter => _bodyParameter.Setup(body))
                      .Header(_header => _header.Setup("Cache-Control", cacheControl))))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}