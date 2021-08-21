using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace NuGetCleaner.Model
{
    [DebuggerDisplay("SearchResult: {this.Data.Title}")]
    public class SearchResult
    {
        public SearchResult()
        {
            this.Data = new List<Data>();
        }

        [JsonProperty("@context")]
        public Context Context { get; set; }

        [JsonProperty("totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty("data")]
        public ICollection<Data> Data { get; set; }
    }
}
