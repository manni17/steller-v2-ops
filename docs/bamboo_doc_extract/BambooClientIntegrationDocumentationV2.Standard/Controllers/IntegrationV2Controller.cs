// <copyright file="IntegrationV2Controller.cs" company="APIMatic">
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
    /// IntegrationV2Controller.
    /// </summary>
    public class IntegrationV2Controller : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationV2Controller"/> class.
        /// </summary>
        internal IntegrationV2Controller(GlobalConfiguration globalConfiguration) : base(globalConfiguration) { }

        /// <summary>
        /// Users have to include valid credentials in the Authorization header of each GET request.
        /// The GET /api/integration/v2.0/catalog endpoint lets you retrieve the list of brands and products available for you.
        /// A successful GET request returns a list of available brands in the response body with the information for each brand.
        /// ### Example response:.
        /// ``` json.
        /// {.
        ///     "pageindex": 0,.
        ///     "pageSize": 100,.
        ///     "count": 1,.
        ///     "items": [.
        ///         {.
        ///             "name": "iTunes USA eGift voucher",.
        ///             "countryCode": "US",.
        ///             "currencyCode": "USD",.
        ///             "description": "string",.
        ///             "disclaimer": "string",.
        ///             "redemptionInstructions": "string",.
        ///             "terms": "string",.
        ///             "logoUrl": "string",.
        ///             "modifiedDate": "2022-08-22T09:06:28.4131483",.
        ///             "products": [.
        ///                 {.
        ///                     "id": 114111,.
        ///                     "name": "iTunes USA eGift voucher",.
        ///                     "minFaceValue": 100.0000,.
        ///                     "maxFaceValue": 100.0000,.
        ///                     "count": null,.
        ///                     "price": {.
        ///                         "min": 100.00000000,.
        ///                         "max": 100.00000000,.
        ///                         "currencyCode": "USD".
        ///                     },.
        ///                     "modifiedDate": "2022-08-22T09:06:29.6783345".
        ///                 }.
        ///             ].
        ///         }.
        ///     ].
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="currencyCode">Required parameter: filter by passing the 3 chars Iso standard currncy format.</param>
        /// <param name="countryCode">Required parameter: filter by passing the 2 chars Iso standard country format.</param>
        /// <param name="name">Required parameter: filter by brand name.</param>
        /// <param name="modifiedDate">Required parameter: filter product modified after spesified date.</param>
        /// <param name="productId">Required parameter: filter only the product with that Id if exist.</param>
        /// <param name="pageSize">Required parameter: page size default: 100.</param>
        /// <param name="pageIndex">Required parameter: page index.</param>
        /// <param name="brandId">Required parameter: filter all product with this brand Id.</param>
        /// <param name="targetCurrency">Required parameter: display all prices in single currency.</param>
        /// <returns>Returns the Models.GetCatalog1 response from the API call.</returns>
        public Models.GetCatalog1 GetCatalog(
                string currencyCode,
                string countryCode,
                string name,
                string modifiedDate,
                int productId,
                int pageSize,
                int pageIndex,
                int brandId,
                string targetCurrency)
            => CoreHelper.RunTask(GetCatalogAsync(currencyCode, countryCode, name, modifiedDate, productId, pageSize, pageIndex, brandId, targetCurrency));

        /// <summary>
        /// Users have to include valid credentials in the Authorization header of each GET request.
        /// The GET /api/integration/v2.0/catalog endpoint lets you retrieve the list of brands and products available for you.
        /// A successful GET request returns a list of available brands in the response body with the information for each brand.
        /// ### Example response:.
        /// ``` json.
        /// {.
        ///     "pageindex": 0,.
        ///     "pageSize": 100,.
        ///     "count": 1,.
        ///     "items": [.
        ///         {.
        ///             "name": "iTunes USA eGift voucher",.
        ///             "countryCode": "US",.
        ///             "currencyCode": "USD",.
        ///             "description": "string",.
        ///             "disclaimer": "string",.
        ///             "redemptionInstructions": "string",.
        ///             "terms": "string",.
        ///             "logoUrl": "string",.
        ///             "modifiedDate": "2022-08-22T09:06:28.4131483",.
        ///             "products": [.
        ///                 {.
        ///                     "id": 114111,.
        ///                     "name": "iTunes USA eGift voucher",.
        ///                     "minFaceValue": 100.0000,.
        ///                     "maxFaceValue": 100.0000,.
        ///                     "count": null,.
        ///                     "price": {.
        ///                         "min": 100.00000000,.
        ///                         "max": 100.00000000,.
        ///                         "currencyCode": "USD".
        ///                     },.
        ///                     "modifiedDate": "2022-08-22T09:06:29.6783345".
        ///                 }.
        ///             ].
        ///         }.
        ///     ].
        /// }.
        ///  ```.
        /// </summary>
        /// <param name="currencyCode">Required parameter: filter by passing the 3 chars Iso standard currncy format.</param>
        /// <param name="countryCode">Required parameter: filter by passing the 2 chars Iso standard country format.</param>
        /// <param name="name">Required parameter: filter by brand name.</param>
        /// <param name="modifiedDate">Required parameter: filter product modified after spesified date.</param>
        /// <param name="productId">Required parameter: filter only the product with that Id if exist.</param>
        /// <param name="pageSize">Required parameter: page size default: 100.</param>
        /// <param name="pageIndex">Required parameter: page index.</param>
        /// <param name="brandId">Required parameter: filter all product with this brand Id.</param>
        /// <param name="targetCurrency">Required parameter: display all prices in single currency.</param>
        /// <param name="cancellationToken"> cancellationToken. </param>
        /// <returns>Returns the Models.GetCatalog1 response from the API call.</returns>
        public async Task<Models.GetCatalog1> GetCatalogAsync(
                string currencyCode,
                string countryCode,
                string name,
                string modifiedDate,
                int productId,
                int pageSize,
                int pageIndex,
                int brandId,
                string targetCurrency,
                CancellationToken cancellationToken = default)
            => await CreateApiCall<Models.GetCatalog1>()
              .RequestBuilder(_requestBuilder => _requestBuilder
                  .Setup(HttpMethod.Get, "/integration/v2.0/catalog")
                  .WithAuth("basic")
                  .Parameters(_parameters => _parameters
                      .Query(_query => _query.Setup("CurrencyCode", currencyCode))
                      .Query(_query => _query.Setup("CountryCode", countryCode))
                      .Query(_query => _query.Setup("Name", name))
                      .Query(_query => _query.Setup("ModifiedDate", modifiedDate))
                      .Query(_query => _query.Setup("ProductId", productId))
                      .Query(_query => _query.Setup("PageSize", pageSize))
                      .Query(_query => _query.Setup("PageIndex", pageIndex))
                      .Query(_query => _query.Setup("BrandId", brandId))
                      .Query(_query => _query.Setup("TargetCurrency", targetCurrency))))
              .ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}