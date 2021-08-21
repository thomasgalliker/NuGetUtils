using System.Collections.Generic;
using NuGetCleaner.Model;

namespace NuGetCleaner.CLI.Tests.Testdata
{
    internal class SearchResults
    {
        internal static SearchResult GetTestSearchResult(string packageId, bool preRelease)
        {
            return new SearchResult
            {
                Data = new List<Data>
                {
                    new Data
                    {
                        Id = packageId,
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