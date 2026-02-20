// <copyright file="Price1.cs" company="APIMatic">
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
    /// Price1.
    /// </summary>
    public class Price1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Price1"/> class.
        /// </summary>
        public Price1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Price1"/> class.
        /// </summary>
        /// <param name="min">min.</param>
        /// <param name="max">max.</param>
        /// <param name="currencyCode">currencyCode.</param>
        public Price1(
            Price1Min min,
            Price1Max max,
            string currencyCode)
        {
            this.Min = min;
            this.Max = max;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Gets or sets Min.
        /// </summary>
        [JsonProperty("min")]
        public Price1Min Min { get; set; }

        /// <summary>
        /// Gets or sets Max.
        /// </summary>
        [JsonProperty("max")]
        public Price1Max Max { get; set; }

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

            return $"Price1 : ({string.Join(", ", toStringOutput)})";
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
            return obj is Price1 other &&                ((this.Min == null && other.Min == null) || (this.Min?.Equals(other.Min) == true)) &&
                ((this.Max == null && other.Max == null) || (this.Max?.Equals(other.Max) == true)) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"Min = {(this.Min == null ? "null" : this.Min.ToString())}");
            toStringOutput.Add($"Max = {(this.Max == null ? "null" : this.Max.ToString())}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
        }
    }
}