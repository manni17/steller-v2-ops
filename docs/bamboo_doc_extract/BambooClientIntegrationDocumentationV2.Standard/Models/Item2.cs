// <copyright file="Item2.cs" company="APIMatic">
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
    /// Item2.
    /// </summary>
    public class Item2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Item2"/> class.
        /// </summary>
        public Item2()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item2"/> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="name">name.</param>
        /// <param name="sku">sku.</param>
        /// <param name="countryCode">countryCode.</param>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="description">description.</param>
        /// <param name="redemptionInstructions">redemptionInstructions.</param>
        /// <param name="terms">terms.</param>
        /// <param name="status">status.</param>
        /// <param name="logoUrl">logoUrl.</param>
        /// <param name="modifiedDate">modifiedDate.</param>
        /// <param name="faceValues">faceValues.</param>
        /// <param name="categories">categories.</param>
        /// <param name="disclaimer">disclaimer.</param>
        public Item2(
            int id,
            string name,
            string sku,
            string countryCode,
            string currencyCode,
            string description,
            string redemptionInstructions,
            string terms,
            string status,
            string logoUrl,
            string modifiedDate,
            List<Item2FaceValues> faceValues,
            List<Item2Categories> categories,
            string disclaimer = null)
        {
            this.Id = id;
            this.Name = name;
            this.Sku = sku;
            this.CountryCode = countryCode;
            this.CurrencyCode = currencyCode;
            this.Description = description;
            this.Disclaimer = disclaimer;
            this.RedemptionInstructions = redemptionInstructions;
            this.Terms = terms;
            this.Status = status;
            this.LogoUrl = logoUrl;
            this.ModifiedDate = modifiedDate;
            this.FaceValues = faceValues;
            this.Categories = categories;
        }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Sku.
        /// </summary>
        [JsonProperty("sku")]
        public string Sku { get; set; }

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
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Disclaimer.
        /// </summary>
        [JsonProperty("disclaimer", NullValueHandling = NullValueHandling.Include)]
        public string Disclaimer { get; set; }

        /// <summary>
        /// Gets or sets RedemptionInstructions.
        /// </summary>
        [JsonProperty("redemptionInstructions")]
        public string RedemptionInstructions { get; set; }

        /// <summary>
        /// Gets or sets Terms.
        /// </summary>
        [JsonProperty("terms")]
        public string Terms { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

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
        /// Gets or sets FaceValues.
        /// </summary>
        [JsonProperty("faceValues")]
        public List<Item2FaceValues> FaceValues { get; set; }

        /// <summary>
        /// Gets or sets Categories.
        /// </summary>
        [JsonProperty("categories")]
        public List<Item2Categories> Categories { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Item2 : ({string.Join(", ", toStringOutput)})";
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
            return obj is Item2 other &&                this.Id.Equals(other.Id) &&
                ((this.Name == null && other.Name == null) || (this.Name?.Equals(other.Name) == true)) &&
                ((this.Sku == null && other.Sku == null) || (this.Sku?.Equals(other.Sku) == true)) &&
                ((this.CountryCode == null && other.CountryCode == null) || (this.CountryCode?.Equals(other.CountryCode) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                ((this.Description == null && other.Description == null) || (this.Description?.Equals(other.Description) == true)) &&
                ((this.Disclaimer == null && other.Disclaimer == null) || (this.Disclaimer?.Equals(other.Disclaimer) == true)) &&
                ((this.RedemptionInstructions == null && other.RedemptionInstructions == null) || (this.RedemptionInstructions?.Equals(other.RedemptionInstructions) == true)) &&
                ((this.Terms == null && other.Terms == null) || (this.Terms?.Equals(other.Terms) == true)) &&
                ((this.Status == null && other.Status == null) || (this.Status?.Equals(other.Status) == true)) &&
                ((this.LogoUrl == null && other.LogoUrl == null) || (this.LogoUrl?.Equals(other.LogoUrl) == true)) &&
                ((this.ModifiedDate == null && other.ModifiedDate == null) || (this.ModifiedDate?.Equals(other.ModifiedDate) == true)) &&
                ((this.FaceValues == null && other.FaceValues == null) || (this.FaceValues?.Equals(other.FaceValues) == true)) &&
                ((this.Categories == null && other.Categories == null) || (this.Categories?.Equals(other.Categories) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Id = {this.Id}");
            toStringOutput.Add($"this.Name = {(this.Name == null ? "null" : this.Name)}");
            toStringOutput.Add($"this.Sku = {(this.Sku == null ? "null" : this.Sku)}");
            toStringOutput.Add($"this.CountryCode = {(this.CountryCode == null ? "null" : this.CountryCode)}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.Description = {(this.Description == null ? "null" : this.Description)}");
            toStringOutput.Add($"this.Disclaimer = {(this.Disclaimer == null ? "null" : this.Disclaimer)}");
            toStringOutput.Add($"this.RedemptionInstructions = {(this.RedemptionInstructions == null ? "null" : this.RedemptionInstructions)}");
            toStringOutput.Add($"this.Terms = {(this.Terms == null ? "null" : this.Terms)}");
            toStringOutput.Add($"this.Status = {(this.Status == null ? "null" : this.Status)}");
            toStringOutput.Add($"this.LogoUrl = {(this.LogoUrl == null ? "null" : this.LogoUrl)}");
            toStringOutput.Add($"this.ModifiedDate = {(this.ModifiedDate == null ? "null" : this.ModifiedDate)}");
            toStringOutput.Add($"FaceValues = {(this.FaceValues == null ? "null" : this.FaceValues.ToString())}");
            toStringOutput.Add($"Categories = {(this.Categories == null ? "null" : this.Categories.ToString())}");
        }
    }
}