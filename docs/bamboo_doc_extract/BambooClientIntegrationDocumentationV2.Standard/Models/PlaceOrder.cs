// <copyright file="PlaceOrder.cs" company="APIMatic">
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
    /// PlaceOrder.
    /// </summary>
    public class PlaceOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceOrder"/> class.
        /// </summary>
        public PlaceOrder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceOrder"/> class.
        /// </summary>
        /// <param name="requestId">requestId.</param>
        /// <param name="orderNumber">orderNumber.</param>
        /// <param name="status">status.</param>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="totalCards">totalCards.</param>
        public PlaceOrder(
            string requestId,
            string orderNumber,
            string status,
            string currencyCode,
            int totalCards)
        {
            this.RequestId = requestId;
            this.OrderNumber = orderNumber;
            this.Status = status;
            this.CurrencyCode = currencyCode;
            this.TotalCards = totalCards;
        }

        /// <summary>
        /// Gets or sets RequestId.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets OrderNumber.
        /// </summary>
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets TotalCards.
        /// </summary>
        [JsonProperty("totalCards")]
        public int TotalCards { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"PlaceOrder : ({string.Join(", ", toStringOutput)})";
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
            return obj is PlaceOrder other &&                ((this.RequestId == null && other.RequestId == null) || (this.RequestId?.Equals(other.RequestId) == true)) &&
                ((this.OrderNumber == null && other.OrderNumber == null) || (this.OrderNumber?.Equals(other.OrderNumber) == true)) &&
                ((this.Status == null && other.Status == null) || (this.Status?.Equals(other.Status) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                this.TotalCards.Equals(other.TotalCards);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.RequestId = {(this.RequestId == null ? "null" : this.RequestId)}");
            toStringOutput.Add($"this.OrderNumber = {(this.OrderNumber == null ? "null" : this.OrderNumber)}");
            toStringOutput.Add($"this.Status = {(this.Status == null ? "null" : this.Status)}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.TotalCards = {this.TotalCards}");
        }
    }
}