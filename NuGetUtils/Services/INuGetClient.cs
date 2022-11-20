using System.Threading.Tasks;
using NuGetUtils.Model;

namespace NuGetUtils.Services
{
    public interface INuGetClient
    {
        /// <summary>
        /// Searches for NuGet packages with given <paramref name="packageId"/>.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        /// <param name="preRelease">Filter for pre-release packages.
        /// If <code>true</code>, only pre-release packages are returned.
        /// If <code>false</code>, only stable packages are returned.
        /// If <code>null</code>, stable and pre-release packages are returned.
        /// </param>
        Task<SearchResult> SearchAsync(string packageId, bool? preRelease, bool skipLatestStable, bool skipLatestPreRelease);

        /// <summary>
        /// Deletes a NuGet package with given <paramref name="packageId"/> and <paramref name="version"/>.
        /// </summary>
        Task DeletePackageAsync(string apiKey, string packageId, string version);

        Task RelistPackageAsync(string apiKey, string packageId, string version);
    }
}