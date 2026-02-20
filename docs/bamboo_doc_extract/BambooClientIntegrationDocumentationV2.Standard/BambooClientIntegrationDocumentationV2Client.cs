// <copyright file="BambooClientIntegrationDocumentationV2Client.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using APIMatic.Core;
using APIMatic.Core.Authentication;
using BambooClientIntegrationDocumentationV2.Standard.Authentication;
using BambooClientIntegrationDocumentationV2.Standard.Controllers;
using BambooClientIntegrationDocumentationV2.Standard.Http.Client;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;

namespace BambooClientIntegrationDocumentationV2.Standard
{
    /// <summary>
    /// The gateway for the SDK. This class acts as a factory for Controller and
    /// holds the configuration of the SDK.
    /// </summary>
    public sealed class BambooClientIntegrationDocumentationV2Client : IConfiguration
    {
        // A map of environments and their corresponding servers/baseurls
        private static readonly Dictionary<Environment, Dictionary<Enum, string>> EnvironmentsMap =
            new Dictionary<Environment, Dictionary<Enum, string>>
        {
            {
                Environment.Production, new Dictionary<Enum, string>
                {
                    { Server.Server1, "http://example.com/api" },
                }
            },
        };

        private readonly GlobalConfiguration globalConfiguration;
        private const string userAgent = "APIMATIC 3.0";
        private readonly HttpCallback httpCallback;
        private readonly Lazy<IntegrationV1Controller> integrationV1;
        private readonly Lazy<IntegrationV2Controller> integrationV2;
        private readonly Lazy<IntegrationV21Controller> integrationV21;

        private BambooClientIntegrationDocumentationV2Client(
            Environment environment,
            BasicAuthModel basicAuthModel,
            HttpCallback httpCallback,
            IHttpClientConfiguration httpClientConfiguration)
        {
            this.Environment = environment;
            this.httpCallback = httpCallback;
            this.HttpClientConfiguration = httpClientConfiguration;
            BasicAuthModel = basicAuthModel;
            var basicAuthManager = new BasicAuthManager(basicAuthModel);
            globalConfiguration = new GlobalConfiguration.Builder()
                .AuthManagers(new Dictionary<string, AuthManager> {
                    {"basic", basicAuthManager},
                })
                .ApiCallback(httpCallback)
                .HttpConfiguration(httpClientConfiguration)
                .ServerUrls(EnvironmentsMap[environment], Server.Server1)
                .UserAgent(userAgent)
                .Build();

            BasicAuthCredentials = basicAuthManager;

            this.integrationV1 = new Lazy<IntegrationV1Controller>(
                () => new IntegrationV1Controller(globalConfiguration));
            this.integrationV2 = new Lazy<IntegrationV2Controller>(
                () => new IntegrationV2Controller(globalConfiguration));
            this.integrationV21 = new Lazy<IntegrationV21Controller>(
                () => new IntegrationV21Controller(globalConfiguration));
        }

        /// <summary>
        /// Gets IntegrationV1Controller controller.
        /// </summary>
        public IntegrationV1Controller IntegrationV1Controller => this.integrationV1.Value;

        /// <summary>
        /// Gets IntegrationV2Controller controller.
        /// </summary>
        public IntegrationV2Controller IntegrationV2Controller => this.integrationV2.Value;

        /// <summary>
        /// Gets IntegrationV21Controller controller.
        /// </summary>
        public IntegrationV21Controller IntegrationV21Controller => this.integrationV21.Value;

        /// <summary>
        /// Gets the configuration of the Http Client associated with this client.
        /// </summary>
        public IHttpClientConfiguration HttpClientConfiguration { get; }

        /// <summary>
        /// Gets Environment.
        /// Current API environment.
        /// </summary>
        public Environment Environment { get; }

        /// <summary>
        /// Gets http callback.
        /// </summary>
        public HttpCallback HttpCallback => this.httpCallback;

        /// <summary>
        /// Gets the credentials to use with BasicAuth.
        /// </summary>
        public IBasicAuthCredentials BasicAuthCredentials { get; private set; }

        /// <summary>
        /// Gets the credentials model to use with BasicAuth.
        /// </summary>
        public BasicAuthModel BasicAuthModel { get; private set; }

        /// <summary>
        /// Gets the URL for a particular alias in the current environment and appends
        /// it with template parameters.
        /// </summary>
        /// <param name="alias">Default value:SERVER 1.</param>
        /// <returns>Returns the baseurl.</returns>
        public string GetBaseUri(Server alias = Server.Server1)
        {
            return globalConfiguration.ServerUrl(alias);
        }

        /// <summary>
        /// Creates an object of the BambooClientIntegrationDocumentationV2Client using the values provided for the builder.
        /// </summary>
        /// <returns>Builder.</returns>
        public Builder ToBuilder()
        {
            Builder builder = new Builder()
                .Environment(this.Environment)
                .HttpCallback(httpCallback)
                .HttpClientConfig(config => config.Build());

            if (BasicAuthModel != null)
            {
                builder.BasicAuthCredentials(BasicAuthModel.ToBuilder().Build());
            }

            return builder;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return
                $"Environment = {this.Environment}, " +
                $"HttpClientConfiguration = {this.HttpClientConfiguration}, ";
        }

        /// <summary>
        /// Creates the client using builder.
        /// </summary>
        /// <returns> BambooClientIntegrationDocumentationV2Client.</returns>
        internal static BambooClientIntegrationDocumentationV2Client CreateFromEnvironment()
        {
            var builder = new Builder();

            string environment = System.Environment.GetEnvironmentVariable("BAMBOO_CLIENT_INTEGRATION_DOCUMENTATION_V_2_STANDARD_ENVIRONMENT");
            string username = System.Environment.GetEnvironmentVariable("BAMBOO_CLIENT_INTEGRATION_DOCUMENTATION_V_2_STANDARD_USERNAME");
            string password = System.Environment.GetEnvironmentVariable("BAMBOO_CLIENT_INTEGRATION_DOCUMENTATION_V_2_STANDARD_PASSWORD");

            if (environment != null)
            {
                builder.Environment(ApiHelper.JsonDeserialize<Environment>($"\"{environment}\""));
            }

            if (username != null && password != null)
            {
                builder.BasicAuthCredentials(new BasicAuthModel
                .Builder(username, password)
                .Build());
            }

            return builder.Build();
        }

        /// <summary>
        /// Builder class.
        /// </summary>
        public class Builder
        {
            private Environment environment = BambooClientIntegrationDocumentationV2.Standard.Environment.Production;
            private BasicAuthModel basicAuthModel = new BasicAuthModel();
            private HttpClientConfiguration.Builder httpClientConfig = new HttpClientConfiguration.Builder();
            private HttpCallback httpCallback;

            /// <summary>
            /// Sets credentials for BasicAuth.
            /// </summary>
            /// <param name="username">Username.</param>
            /// <param name="password">Password.</param>
            /// <returns>Builder.</returns>
            [Obsolete("This method is deprecated. Use BasicAuthCredentials(basicAuthModel) instead.")]
            public Builder BasicAuthCredentials(string username, string password)
            {
                basicAuthModel = basicAuthModel.ToBuilder()
                    .Username(username)
                    .Password(password)
                    .Build();
                return this;
            }

            /// <summary>
            /// Sets credentials for BasicAuth.
            /// </summary>
            /// <param name="basicAuthModel">BasicAuthModel.</param>
            /// <returns>Builder.</returns>
            public Builder BasicAuthCredentials(BasicAuthModel basicAuthModel)
            {
                if (basicAuthModel is null)
                {
                    throw new ArgumentNullException(nameof(basicAuthModel));
                }

                this.basicAuthModel = basicAuthModel;
                return this;
            }

            /// <summary>
            /// Sets Environment.
            /// </summary>
            /// <param name="environment"> Environment. </param>
            /// <returns> Builder. </returns>
            public Builder Environment(Environment environment)
            {
                this.environment = environment;
                return this;
            }

            /// <summary>
            /// Sets HttpClientConfig.
            /// </summary>
            /// <param name="action"> Action. </param>
            /// <returns>Builder.</returns>
            public Builder HttpClientConfig(Action<HttpClientConfiguration.Builder> action)
            {
                if (action is null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                action(this.httpClientConfig);
                return this;
            }



            /// <summary>
            /// Sets the HttpCallback for the Builder.
            /// </summary>
            /// <param name="httpCallback"> http callback. </param>
            /// <returns>Builder.</returns>
            public Builder HttpCallback(HttpCallback httpCallback)
            {
                this.httpCallback = httpCallback;
                return this;
            }

            /// <summary>
            /// Creates an object of the BambooClientIntegrationDocumentationV2Client using the values provided for the builder.
            /// </summary>
            /// <returns>BambooClientIntegrationDocumentationV2Client.</returns>
            public BambooClientIntegrationDocumentationV2Client Build()
            {
                if (basicAuthModel.Username == null || basicAuthModel.Password == null)
                {
                    basicAuthModel = null;
                }
                return new BambooClientIntegrationDocumentationV2Client(
                    environment,
                    basicAuthModel,
                    httpCallback,
                    httpClientConfig.Build());
            }
        }
    }
}
