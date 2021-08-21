namespace NuGetCleaner.CLI.Services
{
    public class NuGetClientConfiguration : INuGetClientConfiguration
    {
        public NuGetClientConfiguration()
        {
        }
        public string ApiKey { get; set; }

    }
    public interface INuGetClientConfiguration
    {
        string ApiKey { get; set; }
    }
}