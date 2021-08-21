using System.CommandLine;
using NuGetCleaner.CLI.Services;

namespace NuGetCleaner.CLI
{
    /// <summary>
    /// A set of <see cref="Option{T}"/> to be shared across different commands.
    /// </summary>
    public static class CommonOptions
    {
        /// <summary>
        /// A shared option that can be used to specify the verbosity of the logging.
        /// It supports the aliases <c>--verbosity</c> and <c>-v</c>.
        /// Valid values are taken from the enumeration <see cref="LogLevel" />.
        /// </summary>
        //public static readonly Option<LogLevel> VerboseOption = new Option<LogLevel>(new[] { "--verbosity", "-v" }, description: "Specify the log verbosity", getDefaultValue: () => LogLevel.Error);

        public static readonly Option<string> ApiKeyOption = new Option<string>(new[] { "--api-key" }, description: "Specify NuGet API key")
        {
            Name = nameof(NuGetClientConfiguration.ApiKey),
            IsRequired = true,
        };

        public static readonly Option<string> PackageIdOption = new Option<string>(new[] { "--package" }, description: "Specify the package ID")
        {
            Name = nameof(PackageIdOption),
            IsRequired = true,
        };

        public static readonly Option<bool> PreReleaseOption = new Option<bool>(new[] { "--pre" }, getDefaultValue: () => false, description: "Filter pre-release packages")
        {
            Name = nameof(PreReleaseOption),
            IsRequired = false,
        };
        
        public static readonly Option<bool> ConfirmOption = new Option<bool>(
            aliases: new[] { "--confirm" }, 
            getDefaultValue: () => false, 
            description: "Filter pre-release packages")
        {
            Name = nameof(ConfirmOption),
            IsRequired = false,
        };


        public static Option<bool> Silent
        {
            get
            {
                return new Option<bool>(
                     alias: "--silent",
                     description: "Silences command output on standard out.",
                     getDefaultValue: () => false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
            }
        }
    }
}