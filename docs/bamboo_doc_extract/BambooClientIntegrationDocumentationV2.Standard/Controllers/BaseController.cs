// <copyright file="BaseController.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using APIMatic.Core;
using APIMatic.Core.Http.Configuration;
using APIMatic.Core.Response;
using BambooClientIntegrationDocumentationV2.Standard.Exceptions;
using BambooClientIntegrationDocumentationV2.Standard.Http.Client;
using BambooClientIntegrationDocumentationV2.Standard.Http.Request;
using BambooClientIntegrationDocumentationV2.Standard.Http.Response;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;
using System;

namespace BambooClientIntegrationDocumentationV2.Standard.Controllers
{
    /// <summary>
    /// The base class for all controller classes.
    /// </summary>
    public class BaseController
    {
        private readonly GlobalConfiguration globalConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        internal BaseController(GlobalConfiguration config) => globalConfiguration = config;

        protected static ErrorCase<HttpRequest, HttpResponse, HttpContext, ApiException> CreateErrorCase(string reason, Func<string, HttpContext, ApiException> error, bool isErrorTemplate = false)
            => new ErrorCase<HttpRequest, HttpResponse, HttpContext, ApiException>(reason, error, isErrorTemplate);

        protected ApiCall<HttpRequest, HttpResponse, HttpContext, ApiException, T, T> CreateApiCall<T>(ArraySerialization arraySerialization = ArraySerialization.Indexed)
            => new ApiCall<HttpRequest, HttpResponse, HttpContext, ApiException, T, T>(
                globalConfiguration,
                compatibilityFactory,
                serialization: arraySerialization
            );

        private static readonly CompatibilityFactory compatibilityFactory = new CompatibilityFactory();
    }
}