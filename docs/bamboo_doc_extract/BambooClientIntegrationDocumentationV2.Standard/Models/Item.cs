// <copyright file="Item.cs" company="APIMatic">
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
    /// Item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="productId">productId.</param>
        /// <param name="productFaceValue">productFaceValue.</param>
        /// <param name="quantity">quantity.</param>
        /// <param name="countryCode">countryCode.</param>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="cards">cards.</param>
        /// <param name="brandCode">brandCode.</param>
        /// <param name="pictureUrl">pictureUrl.</param>
        public Item(
            int productId,
            int productFaceValue,
            int quantity,
            string countryCode,
            string currencyCode,
            List<Models.Card> cards,
            string brandCode = null,
            string pictureUrl = null)
        {
            this.BrandCode = brandCode;
            this.ProductId = productId;
            this.ProductFaceValue = productFaceValue;
            this.Quantity = quantity;
            this.PictureUrl = pictureUrl;
            this.CountryCode = countryCode;
            this.CurrencyCode = currencyCode;
            this.Cards = cards;
        }

        /// <summary>
        /// Gets or sets BrandCode.
        /// </summary>
        [JsonProperty("brandCode", NullValueHandling = NullValueHandling.Include)]
        public string BrandCode { get; set; }

        /// <summary>
        /// Gets or sets ProductId.
        /// </summary>
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets ProductFaceValue.
        /// </summary>
        [JsonProperty("productFaceValue")]
        public int ProductFaceValue { get; set; }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets PictureUrl.
        /// </summary>
        [JsonProperty("pictureUrl", NullValueHandling = NullValueHandling.Include)]
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets CountryCode.
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets Cards.
        /// </summary>
        [JsonProperty("cards")]
        public List<Models.Card> Cards { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Item : ({string.Join(", ", toStringOutput)})";
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
            return obj is Item other &&                ((this.BrandCode == null && other.BrandCode == null) || (this.BrandCode?.Equals(other.BrandCode) == true)) &&
                this.ProductId.Equals(other.ProductId) &&
                this.ProductFaceValue.Equals(other.ProductFaceValue) &&
                this.Quantity.Equals(other.Quantity) &&
                ((this.PictureUrl == null && other.PictureUrl == null) || (this.PictureUrl?.Equals(other.PictureUrl) == true)) &&
                ((this.CountryCode == null && other.CountryCode == null) || (this.CountryCode?.Equals(other.CountryCode) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                ((this.Cards == null && other.Cards == null) || (this.Cards?.Equals(other.Cards) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.BrandCode = {(this.BrandCode == null ? "null" : this.BrandCode)}");
            toStringOutput.Add($"this.ProductId = {this.ProductId}");
            toStringOutput.Add($"this.ProductFaceValue = {this.ProductFaceValue}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.PictureUrl = {(this.PictureUrl == null ? "null" : this.PictureUrl)}");
            toStringOutput.Add($"this.CountryCode = {(this.CountryCode == null ? "null" : this.CountryCode)}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.Cards = {(this.Cards == null ? "null" : $"[{string.Join(", ", this.Cards)} ]")}");
        }
    }
}