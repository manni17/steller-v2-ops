// <copyright file="Transactions.cs" company="APIMatic">
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
    /// Transactions.
    /// </summary>
    public class Transactions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions"/> class.
        /// </summary>
        public Transactions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transactions"/> class.
        /// </summary>
        /// <param name="startDate">startDate.</param>
        /// <param name="endDate">endDate.</param>
        /// <param name="clients">clients.</param>
        public Transactions(
            string startDate,
            string endDate,
            List<Models.Client> clients)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Clients = clients;
        }

        /// <summary>
        /// Gets or sets StartDate.
        /// </summary>
        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets EndDate.
        /// </summary>
        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets Clients.
        /// </summary>
        [JsonProperty("clients")]
        public List<Models.Client> Clients { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Transactions : ({string.Join(", ", toStringOutput)})";
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
            return obj is Transactions other &&                ((this.StartDate == null && other.StartDate == null) || (this.StartDate?.Equals(other.StartDate) == true)) &&
                ((this.EndDate == null && other.EndDate == null) || (this.EndDate?.Equals(other.EndDate) == true)) &&
                ((this.Clients == null && other.Clients == null) || (this.Clients?.Equals(other.Clients) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.StartDate = {(this.StartDate == null ? "null" : this.StartDate)}");
            toStringOutput.Add($"this.EndDate = {(this.EndDate == null ? "null" : this.EndDate)}");
            toStringOutput.Add($"this.Clients = {(this.Clients == null ? "null" : $"[{string.Join(", ", this.Clients)} ]")}");
        }
    }
}