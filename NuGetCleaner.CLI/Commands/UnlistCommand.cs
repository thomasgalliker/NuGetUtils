using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGetCleaner.CLI.Internal;
using NuGetCleaner.CLI.Model;
using NuGetCleaner.CLI.Services;

namespace NuGetCleaner.CLI.Commands
{
    public class UnlistCommand : Command
    {
        public UnlistCommand(ILogger<UnlistCommand> logger, INuGetClient nugetClient) : base(name: "unlist", "Unlists the selected package")
        {
            this.Handler = new UnlistCommandHandler(logger, nugetClient);
            this.AddOption(CommonOptions.ApiKeyOption);
            this.AddOption(CommonOptions.PackageIdOption);
            this.AddOption(CommonOptions.PreReleaseOption);
            this.AddOption(CommonOptions.ConfirmOption);
        }

        private class UnlistCommandHandler : ICommandHandler
        {
            private readonly ILogger<UnlistCommand> logger;
            private readonly INuGetClient nugetClient;

            public UnlistCommandHandler(ILogger<UnlistCommand> logger, INuGetClient nugetClient)
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
                var confirm = context.ParseResult.ValueForOption(CommonOptions.ConfirmOption);
                var searchResult = await this.nugetClient.SearchAsync(packageId, preRelease);

                var count = searchResult.Data.Sum(d => d.Versions.Count);
                if (count > 0)
                {
                    if (!confirm)
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

                    if (confirm || Interactive.Confirmation($"Do you want to unlist {count} package{(count > 1 ? "s" : "")}?", "yes", "no"))
                    {
                        foreach (var data in searchResult.Data)
                        {
                            foreach (var version in data.Versions)
                            {
                                try
                                {
                                    var semanticVersion = SemanticVersion.Parse(version.Version);
                                    await nugetClient.DeletePackageAsync(apiKey, packageId, semanticVersion);
                                }
                                catch (Exception ex)
                                {
                                    this.logger.LogError($"Failed to delete package: {ex.Message}");
                                }
                            }
                        }
                    }
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