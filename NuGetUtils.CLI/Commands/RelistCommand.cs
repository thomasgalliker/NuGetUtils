using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGetUtils.CLI.Internal;
using NuGetUtils.Services;

namespace NuGetUtils.CLI.Commands
{
    public class RelistCommand : Command
    {
        public RelistCommand(ILogger<RelistCommand> logger, INuGetClient nugetClient) : base(name: "relist", "Relist NuGet packages")
        {
            this.Handler = new RelistCommandHandler(logger, nugetClient);
            this.AddOption(CommonOptions.ApiKeyOption);
            this.AddOption(CommonOptions.PackageIdOption);
            this.AddOption(CommonOptions.VersionsOption);
            this.AddOption(CommonOptions.ConfirmOption);
        }

        private class RelistCommandHandler : ICommandHandler
        {
            private readonly ILogger<RelistCommand> logger;
            private readonly INuGetClient nugetClient;

            public RelistCommandHandler(ILogger<RelistCommand> logger, INuGetClient nugetClient)
            {
                this.logger = logger;
                this.nugetClient = nugetClient ?? throw new ArgumentNullException(nameof(nugetClient));
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var apiKey = context.ParseResult.ValueForOption(CommonOptions.ApiKeyOption);
                var packageId = context.ParseResult.ValueForOption(CommonOptions.PackageIdOption);

                var versionsString = context.ParseResult.ValueForOption(CommonOptions.VersionsOption);
                var versions = versionsString?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? new string[0] { };
                
                var confirm = context.ParseResult.ValueForOption(CommonOptions.ConfirmOption);

                var count = versions.Length;
                if (count > 0)
                {
                    if (!confirm)
                    {
                        var searchResultLogMessage = new StringBuilder();
                        foreach (var version in versions)
                        {
                            searchResultLogMessage.AppendLine($"{packageId} {version}");
                        }

                        this.logger.LogInformation(searchResultLogMessage.ToString());
                    }

                    if (confirm || await Interactive.Confirmation($"Do you want to relist {count} package{(count > 1 ? "s" : "")}?", "yes", "no"))
                    {
                        foreach (var version in versions)
                        {
                            try
                            {
                                await this.nugetClient.RelistPackageAsync(apiKey, packageId, version);
                            }
                            catch (Exception ex)
                            {
                                this.logger.LogError($"Failed to relist package: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    this.logger.LogWarning($"No packages found in parameter {CommonOptions.VersionsOption.Aliases.First()}.");
                }

                return 0;
            }
        }
    }
}