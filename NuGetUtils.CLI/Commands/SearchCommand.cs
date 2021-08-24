using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGetUtils.Services;

namespace NuGetUtils.CLI.Commands
{
    public class SearchCommand : Command
    {
        public SearchCommand(ILogger<SearchCommand> logger, INuGetClient nugetClient) : base(name: "search", "Search for NuGet packages")
        {
            this.Handler = new SearchCommandHandler(logger, nugetClient);
            this.AddOption(CommonOptions.ApiKeyOption);
            this.AddOption(CommonOptions.PackageIdOption);
            this.AddOption(CommonOptions.PreReleaseOption);
            this.AddOption(CommonOptions.ConfirmOption);
        }

        private class SearchCommandHandler : ICommandHandler
        {
            private readonly ILogger<SearchCommand> logger;
            private readonly INuGetClient nugetClient;

            public SearchCommandHandler(ILogger<SearchCommand> logger, INuGetClient nugetClient)
            {
                this.logger = logger;
                this.nugetClient = nugetClient ?? throw new ArgumentNullException(nameof(nugetClient));
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var apiKey = context.ParseResult.ValueForOption(CommonOptions.ApiKeyOption);
                var packageId = context.ParseResult.ValueForOption(CommonOptions.PackageIdOption);

                var hasPreRelease = context.ParseResult.Tokens.Any(t => CommonOptions.PreReleaseOption.Aliases.Contains(t.Value));
                var preRelease = hasPreRelease ? context.ParseResult.ValueForOption(CommonOptions.PreReleaseOption) : (bool?)null;

                var searchResult = await this.nugetClient.SearchAsync(packageId, preRelease);

                var count = searchResult.Data.Sum(d => d.Versions.Count);
                if (count > 0)
                {
                    var searchResultLogMessage = new StringBuilder();
                    foreach (var data in searchResult.Data)
                    {
                        foreach (var version in data.Versions)
                        {
                            searchResultLogMessage.AppendLine($"{data.Title} {version.Version}");
                        }
                    }

                    this.logger.LogInformation(searchResultLogMessage.ToString());
                }
                else
                {
                    this.logger.LogWarning($"No packages found for {CommonOptions.PackageIdOption.Aliases.First()}={packageId}, {CommonOptions.PreReleaseOption.Aliases.First()}={preRelease}");
                }

                return 0;
            }
        }
    }
}