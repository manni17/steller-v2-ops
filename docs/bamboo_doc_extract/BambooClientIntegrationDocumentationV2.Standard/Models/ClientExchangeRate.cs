// <copyright file="ClientExchangeRate.cs" company="APIMatic">
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
    /// ClientExchangeRate.
    /// </summary>
    public class ClientExchangeRate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientExchangeRate"/> class.
        /// </summary>
        public ClientExchangeRate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientExchangeRate"/> class.
        /// </summary>
        /// <param name="fromCurrency">fromCurrency.</param>
        /// <param name="toCurrency">toCurrency.</param>
        /// <param name="exchangeRate">exchangeRate.</param>
        public ClientExchangeRate(
            string fromCurrency,
            string toCurrency,
            int exchangeRate)
        {
            this.FromCurrency = fromCurrency;
            this.ToCurrency = toCurrency;
            this.ExchangeRate = exchangeRate;
        }

        /// <summary>
        /// Gets or sets FromCurrency.
        /// </summary>
        [JsonProperty("fromCurrency")]
        public string FromCurrency { get; set; }

        /// <summary>
        /// Gets or sets ToCurrency.
        /// </summary>
        [JsonProperty("toCurrency")]
        public string ToCurrency { get; set; }

        /// <summary>
        /// Gets or sets ExchangeRate.
        /// </summary>
        [JsonProperty("exchangeRate")]
        public int ExchangeRate { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"ClientExchangeRate : ({string.Join(", ", toStringOutput)})";
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
            return obj is ClientExchangeRate other &&                ((this.FromCurrency == null && other.FromCurrency == null) || (this.FromCurrency?.Equals(other.FromCurrency) == true)) &&
                ((this.ToCurrency == null && other.ToCurrency == null) || (this.ToCurrency?.Equals(other.ToCurrency) == true)) &&
                this.ExchangeRate.Equals(other.ExchangeRate);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.FromCurrency = {(this.FromCurrency == null ? "null" : this.FromCurrency)}");
            toStringOutput.Add($"this.ToCurrency = {(this.ToCurrency == null ? "null" : this.ToCurrency)}");
            toStringOutput.Add($"this.ExchangeRate = {this.ExchangeRate}");
        }
    }
}