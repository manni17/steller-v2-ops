// <copyright file="Account.cs" company="APIMatic">
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
    /// Account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        public Account()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="currency">currency.</param>
        /// <param name="balance">balance.</param>
        /// <param name="isActive">isActive.</param>
        /// <param name="sandboxMode">sandboxMode.</param>
        public Account(
            int id,
            string currency,
            AccountBalance balance,
            bool isActive,
            bool sandboxMode)
        {
            this.Id = id;
            this.Currency = currency;
            this.Balance = balance;
            this.IsActive = isActive;
            this.SandboxMode = sandboxMode;
        }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Currency.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets Balance.
        /// </summary>
        [JsonProperty("balance")]
        public AccountBalance Balance { get; set; }

        /// <summary>
        /// Gets or sets IsActive.
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets SandboxMode.
        /// </summary>
        [JsonProperty("sandboxMode")]
        public bool SandboxMode { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Account : ({string.Join(", ", toStringOutput)})";
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
            return obj is Account other &&                this.Id.Equals(other.Id) &&
                ((this.Currency == null && other.Currency == null) || (this.Currency?.Equals(other.Currency) == true)) &&
                ((this.Balance == null && other.Balance == null) || (this.Balance?.Equals(other.Balance) == true)) &&
                this.IsActive.Equals(other.IsActive) &&
                this.SandboxMode.Equals(other.SandboxMode);
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Id = {this.Id}");
            toStringOutput.Add($"this.Currency = {(this.Currency == null ? "null" : this.Currency)}");
            toStringOutput.Add($"Balance = {(this.Balance == null ? "null" : this.Balance.ToString())}");
            toStringOutput.Add($"this.IsActive = {this.IsActive}");
            toStringOutput.Add($"this.SandboxMode = {this.SandboxMode}");
        }
    }
}