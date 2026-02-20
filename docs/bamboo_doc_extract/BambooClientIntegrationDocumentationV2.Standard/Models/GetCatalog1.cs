// <copyright file="GetCatalog1.cs" company="APIMatic">
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
    /// GetCatalog1.
    /// </summary>
    public class GetCatalog1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCatalog1"/> class.
        /// </summary>
        public GetCatalog1()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCatalog1"/> class.
        /// </summary>
        /// <param name="pageIndex">pageIndex.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="count">count.</param>
        /// <param name="items">items.</param>
        public GetCatalog1(
            int pageIndex,
            int pageSize,
            int count,
            List<Models.Item1> items)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Items = items;
        }

        /// <summary>
        /// Gets or sets PageIndex.
        /// </summary>
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets PageSize.
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets Count.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets Items.
        /// </summary>
        [JsonProperty("items")]
        public List<Models.Item1> Items { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var toStringOutput = new List<string>();

            this.ToString(toStringOutput);

            return $"GetCatalog1 : ({string.Join(", ", toStringOutput)})";
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
            return obj is GetCatalog1 other &&                this.PageIndex.Equals(other.PageIndex) &&
                this.PageSize.Equals(other.PageSize) &&
                this.Count.Equals(other.Count) &&
                ((this.Items == null && other.Items == null) || (this.Items?.Equals(other.Items) == true));
        }
        
        /// <summary>
        /// ToString overload.
        /// </summary>
        /// <param name="toStringOutput">List of strings.</param>
        protected void ToString(List<string> toStringOutput)
        {
            toStringOutput.Add($"this.PageIndex = {this.PageIndex}");
            toStringOutput.Add($"this.PageSize = {this.PageSize}");
            toStringOutput.Add($"this.Count = {this.Count}");
            toStringOutput.Add($"this.Items = {(this.Items == null ? "null" : $"[{string.Join(", ", this.Items)} ]")}");
        }
    }
}