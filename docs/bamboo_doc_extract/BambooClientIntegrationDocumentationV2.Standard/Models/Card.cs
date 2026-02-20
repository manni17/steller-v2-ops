// <copyright file="Card.cs" company="APIMatic">
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
    /// Card.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        public Card()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="serialNumber">serialNumber.</param>
        /// <param name="cardCode">cardCode.</param>
        /// <param name="pin">pin.</param>
        /// <param name="expirationDate">expirationDate.</param>
        /// <param name="status">status.</param>
        public Card(
            int id,
            string serialNumber,
            string cardCode,
            string pin,
            string expirationDate,
            string status)
        {
            this.Id = id;
            this.SerialNumber = serialNumber;
            this.CardCode = cardCode;
            this.Pin = pin;
            this.ExpirationDate = expirationDate;
            this.Status = status;
        }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets SerialNumber.
        /// </summary>
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets CardCode.
        /// </summary>
        [JsonProperty("cardCode")]
        public string CardCode { get; set; }

        /// <summary>
        /// Gets or sets Pin.
        /// </summary>
        [JsonProperty("pin")]
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets ExpirationDate.
        /// </summary>
        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Card : ({string.Join(", ", toStringOutput)})";
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
            return obj is Card other &&                this.Id.Equals(other.Id) &&
                ((this.SerialNumber == null && other.SerialNumber == null) || (this.SerialNumber?.Equals(other.SerialNumber) == true)) &&
                ((this.CardCode == null && other.CardCode == null) || (this.CardCode?.Equals(other.CardCode) == true)) &&
                ((this.Pin == null && other.Pin == null) || (this.Pin?.Equals(other.Pin) == true)) &&
                ((this.ExpirationDate == null && other.ExpirationDate == null) || (this.ExpirationDate?.Equals(other.ExpirationDate) == true)) &&
                ((this.Status == null && other.Status == null) || (this.Status?.Equals(other.Status) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Id = {this.Id}");
            toStringOutput.Add($"this.SerialNumber = {(this.SerialNumber == null ? "null" : this.SerialNumber)}");
            toStringOutput.Add($"this.CardCode = {(this.CardCode == null ? "null" : this.CardCode)}");
            toStringOutput.Add($"this.Pin = {(this.Pin == null ? "null" : this.Pin)}");
            toStringOutput.Add($"this.ExpirationDate = {(this.ExpirationDate == null ? "null" : this.ExpirationDate)}");
            toStringOutput.Add($"this.Status = {(this.Status == null ? "null" : this.Status)}");
        }
    }
}