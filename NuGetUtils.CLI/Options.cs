using System.CommandLine;

namespace NuGetUtils.CLI
{
    /// <summary>
    /// A set of <see cref="Option{T}"/> to be shared across different commands.
    /// </summary>
    public static class CommonOptions
    {
        public static readonly Option<string> ApiKeyOption = new Option<string>(
            aliases: new[] { "--api-key" },
            description: "A valid/unrevoked NuGet API key which has the appropriate privileges to run the command.")
        {
            IsRequired = true,
        };

        public static readonly Option<string> PackageIdOption = new Option<string>(
            aliases: new[] { "--package" },
            description: "The NuGet package idientifier.")
        {
            IsRequired = true,
        };

        public static readonly Option<bool> PreReleaseOption = new Option<bool>(
            aliases: new[] { "--pre" }, 
            getDefaultValue: () => true, 
            description: 
            "Filter pre-release packages. " +
            "If true, only pre-release packages are included. " +
            "If false, only stable packages are included. " +
            "If not specified, all (stable and pre-release) packages are included.")
        {
            IsRequired = false,
        };
        
        public static readonly Option<bool> SkipLatestStableOption = new Option<bool>(
            aliases: new[] { "--skip-latest-stable" }, 
            getDefaultValue: () => false, 
            description: 
            "Excludes the latest stable package." +
            "If true, the latest stable version is excluded. " +
            "If false, the latest stable version is included. " +
            "If not specified, the latest stable version is included.")
        {
            IsRequired = false,
        };
        
        public static readonly Option<bool> SkipLatestPreReleaseOption = new Option<bool>(
            aliases: new[] { "--skip-latest-pre" }, 
            getDefaultValue: () => false, 
            description: 
            "Excludes the latest pre-release package." +
            "If true, the latest pre-release version is excluded. " +
            "If false, the latest pre-release version is included. " +
            "If not specified, the latest pre-release version is included.")
        {
            IsRequired = false,
        };
        
        public static readonly Option<string> VersionsOption = new Option<string>(
            aliases: new[] { "--versions" }, 
            description: 
            "A comma-separated list of versions." +
            "Example: 1.0.0-pre.1,2.0.0,3.0.0-alpha.")
        {
            IsRequired = true,
        };
        
        public static readonly Option<bool> ConfirmOption = new Option<bool>(
            aliases: new[] { "--confirm" }, 
            getDefaultValue: () => false, 
            description: "Confirms all user interactions.")
        {
            IsRequired = false,
        };
        
        public static readonly Option<bool> SilentOption = new Option<bool>(
            aliases: new[] { "--silent" }, 
            getDefaultValue: () => false, 
            description: "Silences command output on standard out.")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne
        };
    }
}