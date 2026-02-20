// <copyright file="ExchangeRates.cs" company="APIMatic">
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
    /// ExchangeRates.
    /// </summary>
    public class ExchangeRates
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRates"/> class.
        /// </summary>
        public ExchangeRates()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRates"/> class.
        /// </summary>
        /// <param name="baseCurrencyCode">baseCurrencyCode.</param>
        /// <param name="rates">rates.</param>
        public ExchangeRates(
            string baseCurrencyCode,
            List<Models.Rate> rates)
        {
            this.BaseCurrencyCode = baseCurrencyCode;
            this.Rates = rates;
        }

        /// <summary>
        /// Gets or sets BaseCurrencyCode.
        /// </summary>
        [JsonProperty("baseCurrencyCode")]
        public string BaseCurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets Rates.
        /// </summary>
        [JsonProperty("rates")]
        public List<Models.Rate> Rates { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"ExchangeRates : ({string.Join(", ", toStringOutput)})";
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
            return obj is ExchangeRates other &&                ((this.BaseCurrencyCode == null && other.BaseCurrencyCode == null) || (this.BaseCurrencyCode?.Equals(other.BaseCurrencyCode) == true)) &&
                ((this.Rates == null && other.Rates == null) || (this.Rates?.Equals(other.Rates) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.BaseCurrencyCode = {(this.BaseCurrencyCode == null ? "null" : this.BaseCurrencyCode)}");
            toStringOutput.Add($"this.Rates = {(this.Rates == null ? "null" : $"[{string.Join(", ", this.Rates)} ]")}");
        }
    }
}