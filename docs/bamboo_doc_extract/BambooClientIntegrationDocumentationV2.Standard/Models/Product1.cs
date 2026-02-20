// <copyright file="Product1.cs" company="APIMatic">
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
    /// Product1.
    /// </summary>
    public class Product1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product1"/> class.
        /// </summary>
        public Product1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product1"/> class.
        /// </summary>
        /// <param name="productId">ProductId.</param>
        /// <param name="quantity">Quantity.</param>
        /// <param name="mValue">Value.</param>
        public Product1(
            int productId,
            int quantity,
            int mValue)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
            this.MValue = mValue;
        }

        /// <summary>
        /// Gets or sets ProductId.
        /// </summary>
        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

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

            return $"Product1 : ({string.Join(", ", toStringOutput)})";
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
            return obj is Product1 other &&                this.ProductId.Equals(other.ProductId) &&
                this.Quantity.Equals(other.Quantity) &&
                this.MValue.Equals(other.MValue);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.ProductId = {this.ProductId}");
            toStringOutput.Add($"this.Quantity = {this.Quantity}");
            toStringOutput.Add($"this.MValue = {this.MValue}");
        }
    }
}