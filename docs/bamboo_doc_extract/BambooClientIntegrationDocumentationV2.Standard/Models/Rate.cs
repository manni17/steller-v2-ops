// <copyright file="Rate.cs" company="APIMatic">
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
    /// Rate.
    /// </summary>
    public class Rate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rate"/> class.
        /// </summary>
        public Rate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rate"/> class.
        /// </summary>
        /// <param name="currencyCode">currencyCode.</param>
        /// <param name="mValue">value.</param>
        public Rate(
            string currencyCode,
            double mValue)
        {
            this.CurrencyCode = currencyCode;
            this.MValue = mValue;
        }

        /// <summary>
        /// Gets or sets CurrencyCode.
        /// </summary>
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets MValue.
        /// </summary>
        [JsonProperty("value")]
        public double MValue { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Rate : ({string.Join(", ", toStringOutput)})";
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
            return obj is Rate other &&                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true)) &&
                this.MValue.Equals(other.MValue);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
            toStringOutput.Add($"this.MValue = {this.MValue}");
        }
    }
}