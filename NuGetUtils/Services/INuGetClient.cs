﻿using System.Threading.Tasks;
using NuGetUtils.Model;

namespace NuGetUtils.Services
{
    public interface INuGetClient
    {
        Task<SearchResult> SearchAsync(string packageId, bool? preRelease);

        Task DeletePackageAsync(string apiKey, string packageId, string version);
    }
}