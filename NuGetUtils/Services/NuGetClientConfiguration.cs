namespace NuGetUtils.Services
{
    public class NuGetClientConfiguration : INuGetClientConfiguration
    {
        public NuGetClientConfiguration()
        {
        }

        public string ApiKey { get; set; }
    }
}