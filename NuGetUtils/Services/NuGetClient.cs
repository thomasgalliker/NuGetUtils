using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NuGetUtils.Extensions;
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

        public async Task<SearchResult> SearchAsync(string packageId, bool? preRelease, bool skipLatestStable, bool skipLatestPreRelease)
        {
            if (packageId == null)
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            this.logger.LogInformation($"SearchAsync: packageId={packageId}, preRelease={(preRelease != null ? $"{preRelease}" : "<null>")}, skipLatestStable={skipLatestStable}, skipLatestPreRelease={skipLatestPreRelease}");

            var preReleaseParameter = (preRelease is null or true) ? $"&prerelease=true" : "";
            var uri = $"https://azuresearch-usnc.nuget.org/query?q=packageid:{packageId}{preReleaseParameter}&semVerLevel=2.0.0";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await this.httpClient.SendAsync(request);

            this.logger.LogInformation($"SearchAsync returned StatusCode={(int)response.StatusCode} ({response.StatusCode})");

            var responseString = await response.Content.ReadAsStringAsync();
            var searchResult = JsonConvert.DeserializeObject<SearchResult>(responseString);

            // Filter latest stable version
            if (skipLatestStable)
            {
                foreach (var data in searchResult.Data)
                {
                    var latestStableVersion = data.Versions.LastOrDefault(v => !v.IsPreRelease());
                    if (latestStableVersion != null)
                    {
                        this.logger.LogDebug($"Skipping latest stable version: {latestStableVersion.Version}");
                        data.Versions = data.Versions.Where(v => v != latestStableVersion).ToList();
                    }
                }
            }

            // Filter latest pre-release version
            if (skipLatestPreRelease)
            {
                foreach (var data in searchResult.Data)
                {
                    var latestPreReleaseVersion = data.Versions.LastOrDefault(v => v.IsPreRelease());
                    if (latestPreReleaseVersion != null)
                    {
                        this.logger.LogDebug($"Skipping latest pre-release version: {latestPreReleaseVersion.Version}");
                        data.Versions = data.Versions.Where(v => v != latestPreReleaseVersion).ToList();
                    }
                }
            }

            // Filter pre-release versions
            if (preRelease is bool preReleaseValue)
            {
                foreach (var data in searchResult.Data)
                {
                    var selectedVersions = data.Versions.Select(v =>
                    {
                        var isPreRelease = v.IsPreRelease();
                        return (Version: v, IsPrerelease: isPreRelease);
                    }).Where(x => x.IsPrerelease == preReleaseValue)
                      .Select(x => x.Version)
                      .ToList();

                    data.Versions = selectedVersions;
                }
            }

            return searchResult;
        }

        public async Task DeletePackageAsync(string apiKey, string packageId, string version)
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

            var uri = $"https://www.nuget.org/api/v2/package/{packageId}/{version}";
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
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

        public async Task RelistPackageAsync(string apiKey, string packageId, string version)
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

            this.logger.LogInformation($"RelistPackageAsync for packageId={packageId}, version={version}");

            var uri = $"https://www.nuget.org/api/v2/package/{packageId}/{version}";
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("X-NuGet-ApiKey", apiKey ?? this.apiKey);

            var response = await this.httpClient.SendAsync(request);

            this.logger.LogInformation($"RelistPackageAsync for packageId={packageId}, version={version} returned StatusCode={(int)response.StatusCode} ({response.StatusCode})");

            response.EnsureSuccessStatusCode();
        }
    }
}

