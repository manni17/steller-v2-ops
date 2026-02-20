// <copyright file="Brand.cs" company="APIMatic">
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
using BambooClientIntegrationDocumentationV2.Standard.Models.Containers;
using BambooClientIntegrationDocumentationV2.Standard.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BambooClientIntegrationDocumentationV2.Standard.Models
{
    /// <summary>
    /// Brand.
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brand"/> class.
        /// </summary>
        public Brand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Brand"/> class.
        /// </summary>
        /// <param name="internalId">internalId.</param>
        /// <param name="name">name.</param>
        /// <param name="countryCode">countryCode.</param>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="logoUrl">logoUrl.</param>
        /// <param name="modifiedDate">modifiedDate.</param>
        /// <param name="products">products.</param>
        /// <param name="categories">categories.</param>
        /// <param name="description">description.</param>
        /// <param name="disclaimer">disclaimer.</param>
        /// <param name="redemptionInstructions">redemptionInstructions.</param>
        /// <param name="terms">terms.</param>
        public Brand(
            string internalId,
            string name,
            string countryCode,
            string currencyCode,
            string logoUrl,
            string modifiedDate,
            List<Models.Product> products,
            List<BrandCategories> categories,
            string description = null,
            string disclaimer = null,
            string redemptionInstructions = null,
            string terms = null)
        {
            this.InternalId = internalId;
            this.Name = name;
            this.CountryCode = countryCode;
            this.CurrencyCode = currencyCode;
            this.Description = description;
            this.Disclaimer = disclaimer;
            this.RedemptionInstructions = redemptionInstructions;
            this.Terms = terms;
            this.LogoUrl = logoUrl;
            this.ModifiedDate = modifiedDate;
            this.Products = products;
            this.Categories = categories;
        }

        /// <summary>
        /// Gets or sets InternalId.
        /// </summary>
        [JsonProperty("internalId")]
        public string InternalId { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

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
        /// Gets or sets Description.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Include)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Disclaimer.
        /// </summary>
        [JsonProperty("disclaimer", NullValueHandling = NullValueHandling.Include)]
        public string Disclaimer { get; set; }

        /// <summary>
        /// Gets or sets RedemptionInstructions.
        /// </summary>
        [JsonProperty("redemptionInstructions", NullValueHandling = NullValueHandling.Include)]
        public string RedemptionInstructions { get; set; }

        /// <summary>
        /// Gets or sets Terms.
        /// </summary>
        [JsonProperty("terms", NullValueHandling = NullValueHandling.Include)]
        public string Terms { get; set; }

        /// <summary>
        /// Gets or sets LogoUrl.
        /// </summary>
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        [JsonProperty("modifiedDate")]
        public string ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets Products.
        /// </summary>
        [JsonProperty("products")]
        public List<Models.Product> Products { get; set; }

        /// <summary>
        /// Gets or sets Categories.
        /// </summary>
        [JsonProperty("categories")]
        public List<BrandCategories> Categories { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Brand : ({string.Join(", ", toStringOutput)})";
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
            return obj is Brand other &&                ((this.InternalId == null && other.InternalId == null) || (this.InternalId?.Equals(other.InternalId) == true)) &&
                ((this.Name == null && other.Name == null) || (this.Name?.Equals(other.Name) == true)) &&
                ((this.CountryCode == null && other.CountryCode == null) || (this.CountryCode?.Equals(other.CountryCode) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                ((this.Description == null && other.Description == null) || (this.Description?.Equals(other.Description) == true)) &&
                ((this.Disclaimer == null && other.Disclaimer == null) || (this.Disclaimer?.Equals(other.Disclaimer) == true)) &&
                ((this.RedemptionInstructions == null && other.RedemptionInstructions == null) || (this.RedemptionInstructions?.Equals(other.RedemptionInstructions) == true)) &&
                ((this.Terms == null && other.Terms == null) || (this.Terms?.Equals(other.Terms) == true)) &&
                ((this.LogoUrl == null && other.LogoUrl == null) || (this.LogoUrl?.Equals(other.LogoUrl) == true)) &&
                ((this.ModifiedDate == null && other.ModifiedDate == null) || (this.ModifiedDate?.Equals(other.ModifiedDate) == true)) &&
                ((this.Products == null && other.Products == null) || (this.Products?.Equals(other.Products) == true)) &&
                ((this.Categories == null && other.Categories == null) || (this.Categories?.Equals(other.Categories) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.InternalId = {(this.InternalId == null ? "null" : this.InternalId)}");
            toStringOutput.Add($"this.Name = {(this.Name == null ? "null" : this.Name)}");
            toStringOutput.Add($"this.CountryCode = {(this.CountryCode == null ? "null" : this.CountryCode)}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.Description = {(this.Description == null ? "null" : this.Description)}");
            toStringOutput.Add($"this.Disclaimer = {(this.Disclaimer == null ? "null" : this.Disclaimer)}");
            toStringOutput.Add($"this.RedemptionInstructions = {(this.RedemptionInstructions == null ? "null" : this.RedemptionInstructions)}");
            toStringOutput.Add($"this.Terms = {(this.Terms == null ? "null" : this.Terms)}");
            toStringOutput.Add($"this.LogoUrl = {(this.LogoUrl == null ? "null" : this.LogoUrl)}");
            toStringOutput.Add($"this.ModifiedDate = {(this.ModifiedDate == null ? "null" : this.ModifiedDate)}");
            toStringOutput.Add($"this.Products = {(this.Products == null ? "null" : $"[{string.Join(", ", this.Products)} ]")}");
            toStringOutput.Add($"Categories = {(this.Categories == null ? "null" : this.Categories.ToString())}");
        }
    }
}