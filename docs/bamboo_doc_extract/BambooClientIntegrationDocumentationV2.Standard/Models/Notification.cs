// <copyright file="Notification.cs" company="APIMatic">
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
    /// Notification.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="notificationUrl">notificationUrl.</param>
        /// <param name="notificationUrlSandbox">notificationUrlSandbox.</param>
        /// <param name="secretKey">secretKey.</param>
        public Notification(
            string notificationUrl,
            string notificationUrlSandbox,
            string secretKey)
        {
            this.NotificationUrl = notificationUrl;
            this.NotificationUrlSandbox = notificationUrlSandbox;
            this.SecretKey = secretKey;
        }

        /// <summary>
        /// Gets or sets NotificationUrl.
        /// </summary>
        [JsonProperty("notificationUrl")]
        public string NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets NotificationUrlSandbox.
        /// </summary>
        [JsonProperty("notificationUrlSandbox")]
        public string NotificationUrlSandbox { get; set; }

        /// <summary>
        /// Gets or sets SecretKey.
        /// </summary>
        [JsonProperty("secretKey")]
        public string SecretKey { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"Notification : ({string.Join(", ", toStringOutput)})";
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
            return obj is Notification other &&                ((this.NotificationUrl == null && other.NotificationUrl == null) || (this.NotificationUrl?.Equals(other.NotificationUrl) == true)) &&
                ((this.NotificationUrlSandbox == null && other.NotificationUrlSandbox == null) || (this.NotificationUrlSandbox?.Equals(other.NotificationUrlSandbox) == true)) &&
                ((this.SecretKey == null && other.SecretKey == null) || (this.SecretKey?.Equals(other.SecretKey) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.NotificationUrl = {(this.NotificationUrl == null ? "null" : this.NotificationUrl)}");
            toStringOutput.Add($"this.NotificationUrlSandbox = {(this.NotificationUrlSandbox == null ? "null" : this.NotificationUrlSandbox)}");
            toStringOutput.Add($"this.SecretKey = {(this.SecretKey == null ? "null" : this.SecretKey)}");
        }
    }
}