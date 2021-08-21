using System.Threading.Tasks;
using NuGetCleaner.CLI.Model;
using NuGetCleaner.Model;

namespace NuGetCleaner.Services
{
    public interface INuGetClient
    {
        Task<SearchResult> SearchAsync(string packageId, bool? preRelease);

        Task DeletePackageAsync(string apiKey, string packageId, SemanticVersion version);
    }
}