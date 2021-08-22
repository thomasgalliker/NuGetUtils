using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NuGetUtils.Model
{
    [DebuggerDisplay("Data: {this.Title}")]
    public class Data
    {
        public Data()
        {
            this.Tags = new List<string>();
            this.Authors = new List<string>();
            this.Versions = new List<VersionSummary>();
        }

        [JsonProperty("@id")]
        public Uri Url { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("registration")]
        public Uri Registration { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("licenseUrl")]
        public Uri LicenseUrl { get; set; }

        [JsonProperty("projectUrl")]
        public Uri ProjectUrl { get; set; }

        [JsonProperty("tags")]
        public ICollection<string> Tags { get; set; }

        [JsonProperty("authors")]
        public ICollection<string> Authors { get; set; }

        [JsonProperty("totalDownloads")]
        public long TotalDownloads { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("versions")]
        public ICollection<VersionSummary> Versions { get; set; }
    }
}
