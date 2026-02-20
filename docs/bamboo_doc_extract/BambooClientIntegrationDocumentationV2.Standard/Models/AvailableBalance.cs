// <copyright file="AvailableBalance.cs" company="APIMatic">
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
    /// AvailableBalance.
    /// </summary>
    public class AvailableBalance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableBalance"/> class.
        /// </summary>
        public AvailableBalance()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableBalance"/> class.
        /// </summary>
        /// <param name="mValue">value.</param>
        /// <param name="currencyCode">currencyCode.</param>
        public AvailableBalance(
            double mValue,
            string currencyCode)
        {
            this.MValue = mValue;
            this.CurrencyCode = currencyCode;
        }

        /// <summary>
        /// Gets or sets MValue.
        /// </summary>
        [JsonProperty("value")]
        public double MValue { get; set; }

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

            return $"AvailableBalance : ({string.Join(", ", toStringOutput)})";
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
            return obj is AvailableBalance other &&                this.MValue.Equals(other.MValue) &&
                ((this.CurrencyCode == null && other.CurrencyCode == null) || (this.CurrencyCode?.Equals(other.CurrencyCode) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.MValue = {this.MValue}");
            toStringOutput.Add($"this.CurrencyCode = {(this.CurrencyCode == null ? "null" : this.CurrencyCode)}");
        }
    }
}