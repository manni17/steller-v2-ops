// <copyright file="Transactions1.cs" company="APIMatic">
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
    /// Transactions1.
    /// </summary>
    public class Transactions1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions1"/> class.
        /// </summary>
        public Transactions1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions1"/> class.
        /// </summary>
        /// <param name="orderId">orderId.</param>
        /// <param name="requestId">requestId.</param>
        /// <param name="productId">productId.</param>
        /// <param name="orderNumber">orderNumber.</param>
        /// <param name="orderTotal">orderTotal.</param>
        /// <param name="orderTotalForSelectedProducts">orderTotalForSelectedProducts.</param>
        /// <param name="orderDate">orderDate.</param>
        /// <param name="transactionId">transactionId.</param>
        /// <param name="transactionType">transactionType.</param>
        /// <param name="availableBalance">availableBalance.</param>
        /// <param name="transactionAmount">transactionAmount.</param>
        /// <param name="transactionDate">transactionDate.</param>
        /// <param name="orderItems">orderItems.</param>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="comment">comment.</param>
        /// <param name="notes">notes.</param>
        /// <param name="clientName">clientName.</param>
        /// <param name="orderType">orderType.</param>
        /// <param name="xeroCreditNoteNumber">xeroCreditNoteNumber.</param>
        public Transactions1(
            int orderId,
            string requestId,
            int productId,
            string orderNumber,
            Models.OrderTotal orderTotal,
            Models.OrderTotalForSelectedProducts orderTotalForSelectedProducts,
            string orderDate,
            string transactionId,
            string transactionType,
            Models.AvailableBalance availableBalance,
            Models.TransactionAmount transactionAmount,
            string transactionDate,
            List<Models.OrderItem> orderItems,
            string currencyCode = null,
            string comment = null,
            string notes = null,
            string clientName = null,
            string orderType = null,
            string xeroCreditNoteNumber = null)
        {
            this.OrderId = orderId;
            this.RequestId = requestId;
            this.ProductId = productId;
            this.OrderNumber = orderNumber;
            this.OrderTotal = orderTotal;
            this.OrderTotalForSelectedProducts = orderTotalForSelectedProducts;
            this.CurrencyCode = currencyCode;
            this.OrderDate = orderDate;
            this.TransactionId = transactionId;
            this.TransactionType = transactionType;
            this.Comment = comment;
            this.Notes = notes;
            this.AvailableBalance = availableBalance;
            this.TransactionAmount = transactionAmount;
            this.TransactionDate = transactionDate;
            this.OrderItems = orderItems;
            this.ClientName = clientName;
            this.OrderType = orderType;
            this.XeroCreditNoteNumber = xeroCreditNoteNumber;
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
        /// Gets or sets ProductId.
        /// </summary>
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets OrderNumber.
        /// </summary>
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets OrderTotal.
        /// </summary>
        [JsonProperty("orderTotal")]
        public Models.OrderTotal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets OrderTotalForSelectedProducts.
        /// </summary>
        [JsonProperty("orderTotalForSelectedProducts")]
        public Models.OrderTotalForSelectedProducts OrderTotalForSelectedProducts { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [JsonProperty("currencyCode", NullValueHandling = NullValueHandling.Include)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets OrderDate.
        /// </summary>
        [JsonProperty("orderDate")]
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets TransactionId.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets TransactionType.
        /// </summary>
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets Comment.
        /// </summary>
        [JsonProperty("comment", NullValueHandling = NullValueHandling.Include)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets Notes.
        /// </summary>
        [JsonProperty("notes", NullValueHandling = NullValueHandling.Include)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets AvailableBalance.
        /// </summary>
        [JsonProperty("availableBalance")]
        public Models.AvailableBalance AvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets TransactionAmount.
        /// </summary>
        [JsonProperty("transactionAmount")]
        public Models.TransactionAmount TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets TransactionDate.
        /// </summary>
        [JsonProperty("transactionDate")]
        public string TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets OrderItems.
        /// </summary>
        [JsonProperty("orderItems")]
        public List<Models.OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Gets or sets ClientName.
        /// </summary>
        [JsonProperty("clientName", NullValueHandling = NullValueHandling.Include)]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets OrderType.
        /// </summary>
        [JsonProperty("orderType", NullValueHandling = NullValueHandling.Include)]
        public string OrderType { get; set; }

        /// <summary>
        /// Gets or sets XeroCreditNoteNumber.
        /// </summary>
        [JsonProperty("xeroCreditNoteNumber", NullValueHandling = NullValueHandling.Include)]
        public string XeroCreditNoteNumber { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Transactions1 : ({string.Join(", ", toStringOutput)})";
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
            return obj is Transactions1 other &&                this.OrderId.Equals(other.OrderId) &&
                ((this.RequestId == null && other.RequestId == null) || (this.RequestId?.Equals(other.RequestId) == true)) &&
                this.ProductId.Equals(other.ProductId) &&
                ((this.OrderNumber == null && other.OrderNumber == null) || (this.OrderNumber?.Equals(other.OrderNumber) == true)) &&
                ((this.OrderTotal == null && other.OrderTotal == null) || (this.OrderTotal?.Equals(other.OrderTotal) == true)) &&
                ((this.OrderTotalForSelectedProducts == null && other.OrderTotalForSelectedProducts == null) || (this.OrderTotalForSelectedProducts?.Equals(other.OrderTotalForSelectedProducts) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                ((this.OrderDate == null && other.OrderDate == null) || (this.OrderDate?.Equals(other.OrderDate) == true)) &&
                ((this.TransactionId == null && other.TransactionId == null) || (this.TransactionId?.Equals(other.TransactionId) == true)) &&
                ((this.TransactionType == null && other.TransactionType == null) || (this.TransactionType?.Equals(other.TransactionType) == true)) &&
                ((this.Comment == null && other.Comment == null) || (this.Comment?.Equals(other.Comment) == true)) &&
                ((this.Notes == null && other.Notes == null) || (this.Notes?.Equals(other.Notes) == true)) &&
                ((this.AvailableBalance == null && other.AvailableBalance == null) || (this.AvailableBalance?.Equals(other.AvailableBalance) == true)) &&
                ((this.TransactionAmount == null && other.TransactionAmount == null) || (this.TransactionAmount?.Equals(other.TransactionAmount) == true)) &&
                ((this.TransactionDate == null && other.TransactionDate == null) || (this.TransactionDate?.Equals(other.TransactionDate) == true)) &&
                ((this.OrderItems == null && other.OrderItems == null) || (this.OrderItems?.Equals(other.OrderItems) == true)) &&
                ((this.ClientName == null && other.ClientName == null) || (this.ClientName?.Equals(other.ClientName) == true)) &&
                ((this.OrderType == null && other.OrderType == null) || (this.OrderType?.Equals(other.OrderType) == true)) &&
                ((this.XeroCreditNoteNumber == null && other.XeroCreditNoteNumber == null) || (this.XeroCreditNoteNumber?.Equals(other.XeroCreditNoteNumber) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.OrderId = {this.OrderId}");
            toStringOutput.Add($"this.RequestId = {(this.RequestId == null ? "null" : this.RequestId)}");
            toStringOutput.Add($"this.ProductId = {this.ProductId}");
            toStringOutput.Add($"this.OrderNumber = {(this.OrderNumber == null ? "null" : this.OrderNumber)}");
            toStringOutput.Add($"this.OrderTotal = {(this.OrderTotal == null ? "null" : this.OrderTotal.ToString())}");
            toStringOutput.Add($"this.OrderTotalForSelectedProducts = {(this.OrderTotalForSelectedProducts == null ? "null" : this.OrderTotalForSelectedProducts.ToString())}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.OrderDate = {(this.OrderDate == null ? "null" : this.OrderDate)}");
            toStringOutput.Add($"this.TransactionId = {(this.TransactionId == null ? "null" : this.TransactionId)}");
            toStringOutput.Add($"this.TransactionType = {(this.TransactionType == null ? "null" : this.TransactionType)}");
            toStringOutput.Add($"this.Comment = {(this.Comment == null ? "null" : this.Comment)}");
            toStringOutput.Add($"this.Notes = {(this.Notes == null ? "null" : this.Notes)}");
            toStringOutput.Add($"this.AvailableBalance = {(this.AvailableBalance == null ? "null" : this.AvailableBalance.ToString())}");
            toStringOutput.Add($"this.TransactionAmount = {(this.TransactionAmount == null ? "null" : this.TransactionAmount.ToString())}");
            toStringOutput.Add($"this.TransactionDate = {(this.TransactionDate == null ? "null" : this.TransactionDate)}");
            toStringOutput.Add($"this.OrderItems = {(this.OrderItems == null ? "null" : $"[{string.Join(", ", this.OrderItems)} ]")}");
            toStringOutput.Add($"this.ClientName = {(this.ClientName == null ? "null" : this.ClientName)}");
            toStringOutput.Add($"this.OrderType = {(this.OrderType == null ? "null" : this.OrderType)}");
            toStringOutput.Add($"this.XeroCreditNoteNumber = {(this.XeroCreditNoteNumber == null ? "null" : this.XeroCreditNoteNumber)}");
        }
    }
}