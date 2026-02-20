// <copyright file="Stock.cs" company="APIMatic">
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
    /// Stock.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// </summary>
        public Stock()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// </summary>
        /// <param name="faceValue">faceValue.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="isStocked">isStocked.</param>
        /// <param name="price">price.</param>
        /// <param name="currencyCode">currencyCode.</param>
        public Stock(
            int faceValue,
            int quantity,
            bool isStocked,
            int price,
            string currencyCode)
        {
            this.FaceValue = faceValue;
            this.Quantity = quantity;
            this.IsStocked = isStocked;
            this.Price = price;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Gets or sets FaceValue.
        /// </summary>
        [JsonProperty("faceValue")]
        public int FaceValue { get; set; }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets IsStocked.
        /// </summary>
        [JsonProperty("isStocked")]
        public bool IsStocked { get; set; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        [JsonProperty("price")]
        public int Price { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Stock : ({string.Join(", ", toStringOutput)})";
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
            return obj is Stock other &&                this.FaceValue.Equals(other.FaceValue) &&
                this.Quantity.Equals(other.Quantity) &&
                this.IsStocked.Equals(other.IsStocked) &&
                this.Price.Equals(other.Price) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.FaceValue = {this.FaceValue}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.IsStocked = {this.IsStocked}");
            toStringOutput.Add($"this.Price = {this.Price}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
        }
    }
}