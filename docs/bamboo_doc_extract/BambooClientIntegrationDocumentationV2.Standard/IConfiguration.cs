// <copyright file="IConfiguration.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Net;
using BambooClientIntegrationDocumentationV2.Standard.Authentication;
using BambooClientIntegrationDocumentationV2.Standard.Models;

namespace BambooClientIntegrationDocumentationV2.Standard
{
    /// <summary>
    /// IConfiguration.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets Current API environment.
        /// </summary>
        Environment Environment { get; }

        /// <summary>
        /// Gets the credentials to use with BasicAuth.
        /// </summary>
        IBasicAuthCredentials BasicAuthCredentials { get; }

        /// <summary>
        /// Gets the credentials model to use with BasicAuth.
        /// </summary>
        BasicAuthModel BasicAuthModel { get; }

        /// <summary>
        /// Gets the URL for a particular alias in the current environment and appends it with template parameters.
        /// </summary>
        /// <param name="alias">Default value:SERVER 1.</param>
        /// <returns>Returns the baseurl.</returns>
        string GetBaseUri(Server alias = Server.Server1);
    }
}