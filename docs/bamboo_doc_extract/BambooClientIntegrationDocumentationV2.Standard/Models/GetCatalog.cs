// <copyright file="GetCatalog.cs" company="APIMatic">
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
    /// GetCatalog.
    /// </summary>
    public class GetCatalog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCatalog"/> class.
        /// </summary>
        public GetCatalog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCatalog"/> class.
        /// </summary>
        /// <param name="brands">brands.</param>
        public GetCatalog(
            List<Models.Brand> brands)
        {
            this.Brands = brands;
        }

        /// <summary>
        /// Gets or sets Brands.
        /// </summary>
        [JsonProperty("brands")]
        public List<Models.Brand> Brands { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetCatalog : ({string.Join(", ", toStringOutput)})";
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
            return obj is GetCatalog other &&                ((this.Brands == null && other.Brands == null) || (this.Brands?.Equals(other.Brands) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Brands = {(this.Brands == null ? "null" : $"[{string.Join(", ", this.Brands)} ]")}");
        }
    }
}