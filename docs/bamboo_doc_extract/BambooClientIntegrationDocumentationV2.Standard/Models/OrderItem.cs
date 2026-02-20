// <copyright file="OrderItem.cs" company="APIMatic">
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
    /// OrderItem.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// </summary>
        /// <param name="createdDate">createdDate.</param>
        /// <param name="productName">productName.</param>
        /// <param name="brandName">brandName.</param>
        /// <param name="brandCurrencyCode">brandCurrencyCode.</param>
        /// <param name="brandWithDenominationCurrency">brandWithDenominationCurrency.</param>
        /// <param name="denomination">denomination.</param>
        /// <param name="clientDiscountType">clientDiscountType.</param>
        /// <param name="clientDiscountValue">clientDiscountValue.</param>
        /// <param name="clientTransactionFeeType">clientTransactionFeeType.</param>
        /// <param name="clientTransactionFeeValue">clientTransactionFeeValue.</param>
        /// <param name="clientPrice">clientPrice.</param>
        /// <param name="estimatedPrice">estimatedPrice.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="clientTotal">clientTotal.</param>
        /// <param name="clientExchangeRate">clientExchangeRate.</param>
        public OrderItem(
            string createdDate,
            string productName,
            string brandName,
            string brandCurrencyCode,
            string brandWithDenominationCurrency,
            Models.Denomination denomination,
            string clientDiscountType,
            double clientDiscountValue,
            string clientTransactionFeeType,
            int clientTransactionFeeValue,
            Models.ClientPrice clientPrice,
            Models.EstimatedPrice estimatedPrice,
            int quantity,
            double clientTotal,
            Models.ClientExchangeRate clientExchangeRate)
        {
            this.CreatedDate = createdDate;
            this.ProductName = productName;
            this.BrandName = brandName;
            this.BrandCurrencyCode = brandCurrencyCode;
            this.BrandWithDenominationCurrency = brandWithDenominationCurrency;
            this.Denomination = denomination;
            this.ClientDiscountType = clientDiscountType;
            this.ClientDiscountValue = clientDiscountValue;
            this.ClientTransactionFeeType = clientTransactionFeeType;
            this.ClientTransactionFeeValue = clientTransactionFeeValue;
            this.ClientPrice = clientPrice;
            this.EstimatedPrice = estimatedPrice;
            this.Quantity = quantity;
            this.ClientTotal = clientTotal;
            this.ClientExchangeRate = clientExchangeRate;
        }

        /// <summary>
        /// Gets or sets CreatedDate.
        /// </summary>
        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets ProductName.
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets BrandName.
        /// </summary>
        [JsonProperty("brandName")]
        public string BrandName { get; set; }

        /// <summary>
        /// Gets or sets BrandCurrencyCode.
        /// </summary>
        [JsonProperty("brandCurrencyCode")]
        public string BrandCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets BrandWithDenominationCurrency.
        /// </summary>
        [JsonProperty("brandWithDenominationCurrency")]
        public string BrandWithDenominationCurrency { get; set; }

        /// <summary>
        /// Gets or sets Denomination.
        /// </summary>
        [JsonProperty("denomination")]
        public Models.Denomination Denomination { get; set; }

        /// <summary>
        /// Gets or sets ClientDiscountType.
        /// </summary>
        [JsonProperty("clientDiscountType")]
        public string ClientDiscountType { get; set; }

        /// <summary>
        /// Gets or sets ClientDiscountValue.
        /// </summary>
        [JsonProperty("clientDiscountValue")]
        public double ClientDiscountValue { get; set; }

        /// <summary>
        /// Gets or sets ClientTransactionFeeType.
        /// </summary>
        [JsonProperty("clientTransactionFeeType")]
        public string ClientTransactionFeeType { get; set; }

        /// <summary>
        /// Gets or sets ClientTransactionFeeValue.
        /// </summary>
        [JsonProperty("clientTransactionFeeValue")]
        public int ClientTransactionFeeValue { get; set; }

        /// <summary>
        /// Gets or sets ClientPrice.
        /// </summary>
        [JsonProperty("clientPrice")]
        public Models.ClientPrice ClientPrice { get; set; }

        /// <summary>
        /// Gets or sets EstimatedPrice.
        /// </summary>
        [JsonProperty("estimatedPrice")]
        public Models.EstimatedPrice EstimatedPrice { get; set; }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets ClientTotal.
        /// </summary>
        [JsonProperty("clientTotal")]
        public double ClientTotal { get; set; }

        /// <summary>
        /// Gets or sets ClientExchangeRate.
        /// </summary>
        [JsonProperty("clientExchangeRate")]
        public Models.ClientExchangeRate ClientExchangeRate { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"OrderItem : ({string.Join(", ", toStringOutput)})";
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
            return obj is OrderItem other &&                ((this.CreatedDate == null && other.CreatedDate == null) || (this.CreatedDate?.Equals(other.CreatedDate) == true)) &&
                ((this.ProductName == null && other.ProductName == null) || (this.ProductName?.Equals(other.ProductName) == true)) &&
                ((this.BrandName == null && other.BrandName == null) || (this.BrandName?.Equals(other.BrandName) == true)) &&
                ((this.BrandCurrencyCode == null && other.BrandCurrencyCode == null) || (this.BrandCurrencyCode?.Equals(other.BrandCurrencyCode) == true)) &&
                ((this.BrandWithDenominationCurrency == null && other.BrandWithDenominationCurrency == null) || (this.BrandWithDenominationCurrency?.Equals(other.BrandWithDenominationCurrency) == true)) &&
                ((this.Denomination == null && other.Denomination == null) || (this.Denomination?.Equals(other.Denomination) == true)) &&
                ((this.ClientDiscountType == null && other.ClientDiscountType == null) || (this.ClientDiscountType?.Equals(other.ClientDiscountType) == true)) &&
                this.ClientDiscountValue.Equals(other.ClientDiscountValue) &&
                ((this.ClientTransactionFeeType == null && other.ClientTransactionFeeType == null) || (this.ClientTransactionFeeType?.Equals(other.ClientTransactionFeeType) == true)) &&
                this.ClientTransactionFeeValue.Equals(other.ClientTransactionFeeValue) &&
                ((this.ClientPrice == null && other.ClientPrice == null) || (this.ClientPrice?.Equals(other.ClientPrice) == true)) &&
                ((this.EstimatedPrice == null && other.EstimatedPrice == null) || (this.EstimatedPrice?.Equals(other.EstimatedPrice) == true)) &&
                this.Quantity.Equals(other.Quantity) &&
                this.ClientTotal.Equals(other.ClientTotal) &&
                ((this.ClientExchangeRate == null && other.ClientExchangeRate == null) || (this.ClientExchangeRate?.Equals(other.ClientExchangeRate) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.CreatedDate = {(this.CreatedDate == null ? "null" : this.CreatedDate)}");
            toStringOutput.Add($"this.ProductName = {(this.ProductName == null ? "null" : this.ProductName)}");
            toStringOutput.Add($"this.BrandName = {(this.BrandName == null ? "null" : this.BrandName)}");
            toStringOutput.Add($"this.BrandCurrencyCode = {(this.BrandCurrencyCode == null ? "null" : this.BrandCurrencyCode)}");
            toStringOutput.Add($"this.BrandWithDenominationCurrency = {(this.BrandWithDenominationCurrency == null ? "null" : this.BrandWithDenominationCurrency)}");
            toStringOutput.Add($"this.Denomination = {(this.Denomination == null ? "null" : this.Denomination.ToString())}");
            toStringOutput.Add($"this.ClientDiscountType = {(this.ClientDiscountType == null ? "null" : this.ClientDiscountType)}");
            toStringOutput.Add($"this.ClientDiscountValue = {this.ClientDiscountValue}");
            toStringOutput.Add($"this.ClientTransactionFeeType = {(this.ClientTransactionFeeType == null ? "null" : this.ClientTransactionFeeType)}");
            toStringOutput.Add($"this.ClientTransactionFeeValue = {this.ClientTransactionFeeValue}");
            toStringOutput.Add($"this.ClientPrice = {(this.ClientPrice == null ? "null" : this.ClientPrice.ToString())}");
            toStringOutput.Add($"this.EstimatedPrice = {(this.EstimatedPrice == null ? "null" : this.EstimatedPrice.ToString())}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.ClientTotal = {this.ClientTotal}");
            toStringOutput.Add($"this.ClientExchangeRate = {(this.ClientExchangeRate == null ? "null" : this.ClientExchangeRate.ToString())}");
        }
    }
}