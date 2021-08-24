using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using NuGetUtils.Model;

namespace NuGetUtils.Testdata
{
    public class SearchResults
    {
        public static SearchResult GetCrossplatformLibrary_FodyTracingAspect(bool preRelease)
        {
            var resourceName = preRelease ? "crossplatformlibrary.fodytracingaspect.pre.json" : "crossplatformlibrary.fodytracingaspect.all.json";
            var jsonContent = ResourceLoader.Current.GetEmbeddedResourceString(typeof(SearchResults).Assembly, resourceName);
            return JsonConvert.DeserializeObject<SearchResult>(jsonContent);
        }

        public static SearchResult GetTestSearchResult(string packageId, bool preRelease)
        {
            return new SearchResult
            {
                Data = new List<Data>
                {
                    new Data
                    {
                        Id = packageId,
                        Title = packageId,
                        Versions = new List<VersionSummary>
                        {
                            new VersionSummary
                            {
                                Version = $"1.0.0{(preRelease ? "-pre1" : "")}"
                            },
                            new VersionSummary
                            {
                                Version = $"1.0.0{(preRelease ? "-pre2" : "")}"
                            },
                        }
                    }
                }
            };
        }
    }
}