// <copyright file="Client.cs" company="APIMatic">
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
    /// Client.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="clientName">clientName.</param>
        /// <param name="transactions">transactions.</param>
        public Client(
            string clientName,
            List<Models.Transactions1> transactions)
        {
            this.ClientName = clientName;
            this.Transactions = transactions;
        }

        /// <summary>
        /// Gets or sets ClientName.
        /// </summary>
        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets Transactions.
        /// </summary>
        [JsonProperty("transactions")]
        public List<Models.Transactions1> Transactions { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Client : ({string.Join(", ", toStringOutput)})";
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
            return obj is Client other &&                ((this.ClientName == null && other.ClientName == null) || (this.ClientName?.Equals(other.ClientName) == true)) &&
                ((this.Transactions == null && other.Transactions == null) || (this.Transactions?.Equals(other.Transactions) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.ClientName = {(this.ClientName == null ? "null" : this.ClientName)}");
            toStringOutput.Add($"this.Transactions = {(this.Transactions == null ? "null" : $"[{string.Join(", ", this.Transactions)} ]")}");
        }
    }
}