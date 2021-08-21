using System.CommandLine;
using NuGetCleaner.Services;

namespace NuGetCleaner.CLI
{
    /// <summary>
    /// A set of <see cref="Option{T}"/> to be shared across different commands.
    /// </summary>
    public static class CommonOptions
    {
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
            description: "Confirms all user interactions")
        {
            Name = nameof(ConfirmOption),
            IsRequired = false,
        };
        
        public static readonly Option<bool> SilentOption = new Option<bool>(
            aliases: new[] { "--silent" }, 
            getDefaultValue: () => false, 
            description: "Silences command output on standard out")
        {
            Name = nameof(SilentOption),
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne
        };
    }
}