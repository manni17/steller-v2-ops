// <copyright file="FaceValue.cs" company="APIMatic">
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
    /// FaceValue.
    /// </summary>
    public class FaceValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceValue"/> class.
        /// </summary>
        public FaceValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceValue"/> class.
        /// </summary>
        /// <param name="min">min.</param>
        /// <param name="max">max.</param>
        public FaceValue(
            int? min = null,
            int? max = null)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Gets or sets Min.
        /// </summary>
        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public int? Min { get; set; }

        /// <summary>
        /// Gets or sets Max.
        /// </summary>
        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public int? Max { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"FaceValue : ({string.Join(", ", toStringOutput)})";
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
            return obj is FaceValue other &&                ((this.Min == null && other.Min == null) || (this.Min?.Equals(other.Min) == true)) &&
                ((this.Max == null && other.Max == null) || (this.Max?.Equals(other.Max) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.Min = {(this.Min == null ? "null" : this.Min.ToString())}");
            toStringOutput.Add($"this.Max = {(this.Max == null ? "null" : this.Max.ToString())}");
        }
    }
}