using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NuGetUtils.Model
{
    [DebuggerDisplay("Version: {this.Version}")]
    public class VersionSummary
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("downloads")]
        public long Downloads { get; set; }

        [JsonProperty("@id")]
        public Uri Url { get; set; }
    }
}
