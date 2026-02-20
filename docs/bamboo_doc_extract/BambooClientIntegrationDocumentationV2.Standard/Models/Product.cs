// <copyright file="Product.cs" company="APIMatic">
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
    /// Product.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="name">name.</param>
        /// <param name="minFaceValue">minFaceValue.</param>
        /// <param name="maxFaceValue">maxFaceValue.</param>
        /// <param name="price">price.</param>
        /// <param name="modifiedDate">modifiedDate.</param>
        /// <param name="count">count.</param>
        public Product(
            int id,
            string name,
            ProductMinFaceValue minFaceValue,
            ProductMaxFaceValue maxFaceValue,
            Models.Price price,
            string modifiedDate,
            ProductCount count = null)
        {
            this.Id = id;
            this.Name = name;
            this.MinFaceValue = minFaceValue;
            this.MaxFaceValue = maxFaceValue;
            this.Count = count;
            this.Price = price;
            this.ModifiedDate = modifiedDate;
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
        /// Gets or sets MinFaceValue.
        /// </summary>
        [JsonProperty("minFaceValue")]
        public ProductMinFaceValue MinFaceValue { get; set; }

        /// <summary>
        /// Gets or sets MaxFaceValue.
        /// </summary>
        [JsonProperty("maxFaceValue")]
        public ProductMaxFaceValue MaxFaceValue { get; set; }

        /// <summary>
        /// Gets or sets Count.
        /// </summary>
        [JsonProperty("count", NullValueHandling = NullValueHandling.Include)]
        public ProductCount Count { get; set; }

        /// <summary>
        /// Gets or sets Price.
        /// </summary>
        [JsonProperty("price")]
        public Models.Price Price { get; set; }

        /// <summary>
        /// Gets or sets ModifiedDate.
        /// </summary>
        [JsonProperty("modifiedDate")]
        public string ModifiedDate { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Product : ({string.Join(", ", toStringOutput)})";
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
            return obj is Product other &&                this.Id.Equals(other.Id) &&
                ((this.Name == null && other.Name == null) || (this.Name?.Equals(other.Name) == true)) &&
                ((this.MinFaceValue == null && other.MinFaceValue == null) || (this.MinFaceValue?.Equals(other.MinFaceValue) == true)) &&
                ((this.MaxFaceValue == null && other.MaxFaceValue == null) || (this.MaxFaceValue?.Equals(other.MaxFaceValue) == true)) &&
                ((this.Count == null && other.Count == null) || (this.Count?.Equals(other.Count) == true)) &&
                ((this.Price == null && other.Price == null) || (this.Price?.Equals(other.Price) == true)) &&
                ((this.ModifiedDate == null && other.ModifiedDate == null) || (this.ModifiedDate?.Equals(other.ModifiedDate) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Id = {this.Id}");
            toStringOutput.Add($"this.Name = {(this.Name == null ? "null" : this.Name)}");
            toStringOutput.Add($"MinFaceValue = {(this.MinFaceValue == null ? "null" : this.MinFaceValue.ToString())}");
            toStringOutput.Add($"MaxFaceValue = {(this.MaxFaceValue == null ? "null" : this.MaxFaceValue.ToString())}");
            toStringOutput.Add($"Count = {(this.Count == null ? "null" : this.Count.ToString())}");
            toStringOutput.Add($"this.Price = {(this.Price == null ? "null" : this.Price.ToString())}");
            toStringOutput.Add($"this.ModifiedDate = {(this.ModifiedDate == null ? "null" : this.ModifiedDate)}");
        }
    }
}