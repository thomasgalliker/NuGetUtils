using NuGet.Versioning;
using NuGetUtils.Model;

namespace NuGetUtils.Extensions
{
    public static class VersionSummaryExtensions
    {
        public static bool IsPreRelease(this VersionSummary v)
        {
            var isPreRelease = false;

            if (SemanticVersion.TryParse(v.Version, out var semanticVersion))
            {
                isPreRelease = semanticVersion.IsPrerelease;
            }
            else if (v.Version.IndexOf('-') > 0)
            {
                isPreRelease = true;
            }

            return isPreRelease;
        }
    }
}
