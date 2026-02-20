// <copyright file="Order.cs" company="APIMatic">
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
    /// Order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="clientName">clientName.</param>
        /// <param name="orderNumber">orderNumber.</param>
        /// <param name="orderDate">orderDate.</param>
        /// <param name="clientReferenceNumber">clientReferenceNumber.</param>
        /// <param name="channel">channel.</param>
        /// <param name="productName">productName.</param>
        /// <param name="denomination">denomination.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="unitPrice">unitPrice.</param>
        /// <param name="subTotal">subTotal.</param>
        /// <param name="total">total.</param>
        /// <param name="accountCurrency">accountCurrency.</param>
        /// <param name="status">status.</param>
        public Order(
            string clientName,
            string orderNumber,
            string orderDate,
            string clientReferenceNumber,
            string channel,
            string productName,
            string denomination,
            int quantity,
            double unitPrice,
            double subTotal,
            double total,
            string accountCurrency,
            string status)
        {
            this.ClientName = clientName;
            this.OrderNumber = orderNumber;
            this.OrderDate = orderDate;
            this.ClientReferenceNumber = clientReferenceNumber;
            this.Channel = channel;
            this.ProductName = productName;
            this.Denomination = denomination;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.SubTotal = subTotal;
            this.Total = total;
            this.AccountCurrency = accountCurrency;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets ClientName.
        /// </summary>
        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets OrderNumber.
        /// </summary>
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets OrderDate.
        /// </summary>
        [JsonProperty("orderDate")]
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets ClientReferenceNumber.
        /// </summary>
        [JsonProperty("clientReferenceNumber")]
        public string ClientReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets Channel.
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets ProductName.
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets Denomination.
        /// </summary>
        [JsonProperty("denomination")]
        public string Denomination { get; set; }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets UnitPrice.
        /// </summary>
        [JsonProperty("unitPrice")]
        public double UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets SubTotal.
        /// </summary>
        [JsonProperty("subTotal")]
        public double SubTotal { get; set; }

        /// <summary>
        /// Gets or sets Total.
        /// </summary>
        [JsonProperty("total")]
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets AccountCurrency.
        /// </summary>
        [JsonProperty("accountCurrency")]
        public string AccountCurrency { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Order : ({string.Join(", ", toStringOutput)})";
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
            return obj is Order other &&                ((this.ClientName == null && other.ClientName == null) || (this.ClientName?.Equals(other.ClientName) == true)) &&
                ((this.OrderNumber == null && other.OrderNumber == null) || (this.OrderNumber?.Equals(other.OrderNumber) == true)) &&
                ((this.OrderDate == null && other.OrderDate == null) || (this.OrderDate?.Equals(other.OrderDate) == true)) &&
                ((this.ClientReferenceNumber == null && other.ClientReferenceNumber == null) || (this.ClientReferenceNumber?.Equals(other.ClientReferenceNumber) == true)) &&
                ((this.Channel == null && other.Channel == null) || (this.Channel?.Equals(other.Channel) == true)) &&
                ((this.ProductName == null && other.ProductName == null) || (this.ProductName?.Equals(other.ProductName) == true)) &&
                ((this.Denomination == null && other.Denomination == null) || (this.Denomination?.Equals(other.Denomination) == true)) &&
                this.Quantity.Equals(other.Quantity) &&
                this.UnitPrice.Equals(other.UnitPrice) &&
                this.SubTotal.Equals(other.SubTotal) &&
                this.Total.Equals(other.Total) &&
                ((this.AccountCurrency == null && other.AccountCurrency == null) || (this.AccountCurrency?.Equals(other.AccountCurrency) == true)) &&
                ((this.Status == null && other.Status == null) || (this.Status?.Equals(other.Status) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.ClientName = {(this.ClientName == null ? "null" : this.ClientName)}");
            toStringOutput.Add($"this.OrderNumber = {(this.OrderNumber == null ? "null" : this.OrderNumber)}");
            toStringOutput.Add($"this.OrderDate = {(this.OrderDate == null ? "null" : this.OrderDate)}");
            toStringOutput.Add($"this.ClientReferenceNumber = {(this.ClientReferenceNumber == null ? "null" : this.ClientReferenceNumber)}");
            toStringOutput.Add($"this.Channel = {(this.Channel == null ? "null" : this.Channel)}");
            toStringOutput.Add($"this.ProductName = {(this.ProductName == null ? "null" : this.ProductName)}");
            toStringOutput.Add($"this.Denomination = {(this.Denomination == null ? "null" : this.Denomination)}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.UnitPrice = {this.UnitPrice}");
            toStringOutput.Add($"this.SubTotal = {this.SubTotal}");
            toStringOutput.Add($"this.Total = {this.Total}");
            toStringOutput.Add($"this.AccountCurrency = {(this.AccountCurrency == null ? "null" : this.AccountCurrency)}");
            toStringOutput.Add($"this.Status = {(this.Status == null ? "null" : this.Status)}");
        }
    }
}