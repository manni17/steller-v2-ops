// <copyright file="PlaceOrderRequest1.cs" company="APIMatic">
// Copyright (c) APIMatic. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIMatic.Core.Utilities.Converters;
using BambooClientIntegrationDocumentationV2.Standard;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BambooClientIntegrationDocumentationV2.Standard.Models
{
    /// <summary>
    /// PlaceOrderRequest1.
    /// </summary>
    public class PlaceOrderRequest1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceOrderRequest1"/> class.
        /// </summary>
        public PlaceOrderRequest1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceOrderRequest1"/> class.
        /// </summary>
        /// <param name="requestId">RequestId.</param>
        /// <param name="accountId">AccountId.</param>
        /// <param name="products">Products.</param>
        public PlaceOrderRequest1(
            string requestId,
            int accountId,
            List<Models.Product3> products)
        {
            this.RequestId = requestId;
            this.AccountId = accountId;
            this.Products = products;
        }

        /// <summary>
        /// Gets or sets RequestId.
        /// </summary>
        [JsonProperty("RequestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets AccountId.
        /// </summary>
        [JsonProperty("AccountId")]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets Products.
        /// </summary>
        [JsonProperty("Products")]
        public List<Models.Product3> Products { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"PlaceOrderRequest1 : ({string.Join(", ", toStringOutput)})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj == this)
            {
                return true;
            }
            return obj is PlaceOrderRequest1 other &&                ((this.RequestId == null && other.RequestId == null) || (this.RequestId?.Equals(other.RequestId) == true)) &&
                this.AccountId.Equals(other.AccountId) &&
                ((this.Products == null && other.Products == null) || (this.Products?.Equals(other.Products) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.RequestId = {(this.RequestId == null ? "null" : this.RequestId)}");
            toStringOutput.Add($"this.AccountId = {this.AccountId}");
            toStringOutput.Add($"this.Products = {(this.Products == null ? "null" : $"[{string.Join(", ", this.Products)} ]")}");
        }
    }
}