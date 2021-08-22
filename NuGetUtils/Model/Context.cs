using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NuGetUtils.Model
{
    [DebuggerDisplay("Context: {this.Base}")]
    public partial class Context
    {
        [JsonProperty("@vocab")]
        public Uri Vocab { get; set; }

        [JsonProperty("@base")]
        public Uri Base { get; set; }
    }
}
