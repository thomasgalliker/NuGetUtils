using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NuGetCleaner.CLI.Commands;
using NuGetCleaner.Services;

namespace NuGetCleaner.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();
            var parser = BuildParser(serviceProvider);
            return await parser.InvokeAsync(args).ConfigureAwait(false);
        }

        private static Parser BuildParser(IServiceProvider serviceProvider)
        {
            var rootCommand = new RootCommand();
            rootCommand.Description = 
                $"Unlists NuGet packages using command line arguments. {Environment.NewLine}" +
                $"(c){DateTime.Now.Year} superdev GmbH. All rights reserved.{Environment.NewLine}";

            rootCommand.AddGlobalOption(CommonOptions.Silent);

            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            var commands = serviceProvider.GetServices<Command>();
            foreach (var command in commands)
            {
                commandLineBuilder.AddCommand(command);
            }

            return commandLineBuilder
                .UseDefaults()
                .Build();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.SetMinimumLevel(LogLevel.Debug);
                configure.AddDebug();
                configure.AddSimpleConsole(o =>
                {
                    o.SingleLine = false;
                    o.TimestampFormat = "hh:mm:ss ";
                });
            });

            services.AddSingleton<Command, UnlistCommand>();
            services.AddSingleton<INuGetClient, NuGetClient>();
            services.AddSingleton<INuGetClientConfiguration, NuGetClientConfiguration>();

            return services.BuildServiceProvider();
        }
    }
}
