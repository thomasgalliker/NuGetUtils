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
            description: "Specify NuGet API key")
        {
            IsRequired = true,
        };

        public static readonly Option<string> PackageIdOption = new Option<string>(
            aliases: new[] { "--package" },
            description: "Specify the package ID")
        {
            IsRequired = true,
        };

        public static readonly Option<bool> PreReleaseOption = new Option<bool>(
            aliases: new[] { "--pre" }, 
            getDefaultValue: () => false, 
            description: "Filter pre-release packages")
        {
            IsRequired = false,
        };
        
        public static readonly Option<bool> ConfirmOption = new Option<bool>(
            aliases: new[] { "--confirm" }, 
            getDefaultValue: () => false, 
            description: "Confirms all user interactions")
        {
            IsRequired = false,
        };
        
        public static readonly Option<bool> SilentOption = new Option<bool>(
            aliases: new[] { "--silent" }, 
            getDefaultValue: () => false, 
            description: "Silences command output on standard out")
        {
            IsRequired = false,
            Arity = ArgumentArity.ZeroOrOne
        };
    }
}