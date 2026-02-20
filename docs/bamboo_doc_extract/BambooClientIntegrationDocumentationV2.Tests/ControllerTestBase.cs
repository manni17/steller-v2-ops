// <copyright file="ControllerTestBase.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BambooClientIntegrationDocumentationV2.Standard;
using BambooClientIntegrationDocumentationV2.Standard.Authentication;
using BambooClientIntegrationDocumentationV2.Standard.Http.Client;
using BambooClientIntegrationDocumentationV2.Standard.Models;
using BambooClientIntegrationDocumentationV2.Standard.Models.Containers;
using NUnit.Framework;

namespace BambooClientIntegrationDocumentationV2.Tests
{
    /// <summary>
    /// ControllerTestBase Class.
    /// </summary>
    [TestFixture]
    public class ControllerTestBase
    {
        /// <summary>
        /// Assert precision.
        /// </summary>
        protected const double AssertPrecision = 0.1;

        /// <summary>
        /// Gets HttpCallBackHandler.
        /// </summary>
        internal HttpCallback HttpCallBack { get; private set; } = new HttpCallback();

        /// <summary>
        /// Gets BambooClientIntegrationDocumentationV2Client Client.
        /// </summary>
        protected BambooClientIntegrationDocumentationV2Client Client { get; private set; }

        /// <summary>
        /// Set up the client.
        /// </summary>
        [OneTimeSetUp]
        public void SetUp()
        {
            BambooClientIntegrationDocumentationV2Client config = BambooClientIntegrationDocumentationV2Client.CreateFromEnvironment();
            this.Client = config.ToBuilder()
                .HttpCallback(HttpCallBack)
                .Build();
        }
    }
}