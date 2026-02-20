// <copyright file="Product3.cs" company="APIMatic">
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
    /// Product3.
    /// </summary>
    public class Product3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product3"/> class.
        /// </summary>
        public Product3()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product3"/> class.
        /// </summary>
        /// <param name="sku">Sku.</param>
        /// <param name="quantity">Quantity.</param>
        /// <param name="mValue">Value.</param>
        public Product3(
            string sku,
            int quantity,
            int mValue)
        {
            this.Sku = sku;
            this.Quantity = quantity;
            this.MValue = mValue;
        }

        /// <summary>
        /// Gets or sets Sku.
        /// </summary>
        [JsonProperty("Sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets Quantity.
        /// </summary>
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets MValue.
        /// </summary>
        [JsonProperty("Value")]
        public int MValue { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Product3 : ({string.Join(", ", toStringOutput)})";
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
            return obj is Product3 other &&                ((this.Sku == null && other.Sku == null) || (this.Sku?.Equals(other.Sku) == true)) &&
                this.Quantity.Equals(other.Quantity) &&
                this.MValue.Equals(other.MValue);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Sku = {(this.Sku == null ? "null" : this.Sku)}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.MValue = {this.MValue}");
        }
    }
}