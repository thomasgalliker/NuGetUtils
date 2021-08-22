﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGetUtils.CLI.Model;
using NuGetUtils.Model;

namespace NuGetUtils.Services
{
    public class NuGetClient : INuGetClient
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly string apiKey;

        public NuGetClient(ILogger<NuGetClient> logger, INuGetClientConfiguration configuration)
        {
            this.logger = logger;
            this.httpClient ??= new HttpClient();
            this.apiKey = configuration.ApiKey;
        }

        public async Task<SearchResult> SearchAsync(string packageId, bool? preRelease)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            this.logger.LogInformation($"SearchAsync: packageId={packageId}, preRelease={preRelease}");

            var preReleaseParameter = preRelease != null ? $"&prerelease={Convert.ToString(preRelease)}" : "";
            var uri = $"https://azuresearch-usnc.nuget.org/query?q=packageid:{packageId}{preReleaseParameter}&semVerLevel=2.0.0";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await this.httpClient.SendAsync(request);

            this.logger.LogInformation($"SearchAsync returned StatusCode={(int)response.StatusCode} ({response.StatusCode})");

            var responseString = await response.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<SearchResult>(responseString);

            // Filter pre-release versions
            if (preRelease is bool preReleaseValue)
            {
                foreach (var data in searchResult.Data)
                {
                    var preReleaseVersions = data.Versions.Select(v =>
                    {
                        var semanticVersion = SemanticVersion.Parse(v.Version);
                        return (Version: v, SemanticVersion: semanticVersion);
                    }).Where(x => x.SemanticVersion.IsPreRelease == preReleaseValue)
                      .Select(x => x.Version)
                      .ToList();

                    data.Versions = preReleaseVersions;
                }
            }

            return searchResult;
        }

        public async Task DeletePackageAsync(string apiKey, string packageId, SemanticVersion version)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            this.logger.LogInformation($"DeletePackageAsync for packageId={packageId}, version={version}");

            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://www.nuget.org/api/v2/package/{packageId}/{version}");
            request.Headers.Add("X-NuGet-ApiKey", apiKey ?? this.apiKey);

            var response = await this.httpClient.SendAsync(request);

            this.logger.LogInformation($"DeletePackageAsync for packageId={packageId}, version={version} returned StatusCode={(int)response.StatusCode} ({response.StatusCode})");

            response.EnsureSuccessStatusCode();

            ////switch (response.StatusCode)
            ////{
            ////    case HttpStatusCode.OK:
            ////    case HttpStatusCode.Accepted:

            ////    case HttpStatusCode.Unauthorized:
            ////        throw new UnauthorizedAccessException(response.ReasonPhrase);
            ////    case HttpStatusCode.Forbidden:
            ////        // https://docs.microsoft.com/en-us/nuget/api/rate-limits
            ////        throw new UnauthorizedAccessException(response.ReasonPhrase);
            ////    case HttpStatusCode.NotFound:
            ////        throw new VersionNotFoundException(response.ReasonPhrase);
            ////    default:
            ////        throw new HttpRequestException(
            ////            $"{response.StatusCode}: {response.ReasonPhrase}");
            ////}
        }
    }
}
