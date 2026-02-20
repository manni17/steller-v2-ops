// <copyright file="GetOrder.cs" company="APIMatic">
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
    /// GetOrder.
    /// </summary>
    public class GetOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrder"/> class.
        /// </summary>
        public GetOrder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetOrder"/> class.
        /// </summary>
        /// <param name="orderId">orderId.</param>
        /// <param name="requestId">requestId.</param>
        /// <param name="items">items.</param>
        /// <param name="status">status.</param>
        /// <param name="createdDate">createdDate.</param>
        /// <param name="total">total.</param>
        /// <param name="orderType">orderType.</param>
        /// <param name="currency">currency.</param>
        /// <param name="errorMessage">errorMessage.</param>
        public GetOrder(
            int orderId,
            string requestId,
            List<Models.Item> items,
            string status,
            string createdDate,
            double total,
            string orderType,
            string currency,
            string errorMessage = null)
        {
            this.OrderId = orderId;
            this.RequestId = requestId;
            this.Items = items;
            this.Status = status;
            this.CreatedDate = createdDate;
            this.Total = total;
            this.ErrorMessage = errorMessage;
            this.OrderType = orderType;
            this.Currency = currency;
        }

        /// <summary>
        /// Gets or sets OrderId.
        /// </summary>
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets RequestId.
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets Items.
        /// </summary>
        [JsonProperty("items")]
        public List<Models.Item> Items { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets CreatedDate.
        /// </summary>
        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets Total.
        /// </summary>
        [JsonProperty("total")]
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets ErrorMessage.
        /// </summary>
        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Include)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets OrderType.
        /// </summary>
        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        /// <summary>
        /// Gets or sets Currency.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetOrder : ({string.Join(", ", toStringOutput)})";
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
            return obj is GetOrder other &&                this.OrderId.Equals(other.OrderId) &&
                ((this.RequestId == null && other.RequestId == null) || (this.RequestId?.Equals(other.RequestId) == true)) &&
                ((this.Items == null && other.Items == null) || (this.Items?.Equals(other.Items) == true)) &&
                ((this.Status == null && other.Status == null) || (this.Status?.Equals(other.Status) == true)) &&
                ((this.CreatedDate == null && other.CreatedDate == null) || (this.CreatedDate?.Equals(other.CreatedDate) == true)) &&
                this.Total.Equals(other.Total) &&
                ((this.ErrorMessage == null && other.ErrorMessage == null) || (this.ErrorMessage?.Equals(other.ErrorMessage) == true)) &&
                ((this.OrderType == null && other.OrderType == null) || (this.OrderType?.Equals(other.OrderType) == true)) &&
                ((this.Currency == null && other.Currency == null) || (this.Currency?.Equals(other.Currency) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.OrderId = {this.OrderId}");
            toStringOutput.Add($"this.RequestId = {(this.RequestId == null ? "null" : this.RequestId)}");
            toStringOutput.Add($"this.Items = {(this.Items == null ? "null" : $"[{string.Join(", ", this.Items)} ]")}");
            toStringOutput.Add($"this.Status = {(this.Status == null ? "null" : this.Status)}");
            toStringOutput.Add($"this.CreatedDate = {(this.CreatedDate == null ? "null" : this.CreatedDate)}");
            toStringOutput.Add($"this.Total = {this.Total}");
            toStringOutput.Add($"this.ErrorMessage = {(this.ErrorMessage == null ? "null" : this.ErrorMessage)}");
            toStringOutput.Add($"this.OrderType = {(this.OrderType == null ? "null" : this.OrderType)}");
            toStringOutput.Add($"this.Currency = {(this.Currency == null ? "null" : this.Currency)}");
        }
    }
}