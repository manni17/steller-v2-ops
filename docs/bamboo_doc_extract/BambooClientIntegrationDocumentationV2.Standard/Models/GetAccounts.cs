// <copyright file="GetAccounts.cs" company="APIMatic">
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
    /// GetAccounts.
    /// </summary>
    public class GetAccounts
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccounts"/> class.
        /// </summary>
        public GetAccounts()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAccounts"/> class.
        /// </summary>
        /// <param name="accounts">accounts.</param>
        public GetAccounts(
            List<Models.Account> accounts)
        {
            this.Accounts = accounts;
        }

        /// <summary>
        /// Gets or sets Accounts.
        /// </summary>
        [JsonProperty("accounts")]
        public List<Models.Account> Accounts { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetAccounts : ({string.Join(", ", toStringOutput)})";
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
            return obj is GetAccounts other &&                ((this.Accounts == null && other.Accounts == null) || (this.Accounts?.Equals(other.Accounts) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Accounts = {(this.Accounts == null ? "null" : $"[{string.Join(", ", this.Accounts)} ]")}");
        }
    }
}