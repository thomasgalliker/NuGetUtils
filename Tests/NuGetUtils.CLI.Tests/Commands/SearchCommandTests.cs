using System.CommandLine;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NuGetUtils.CLI.Commands;
using NuGetUtils.Model;
using NuGetUtils.Services;
using NuGetUtils.Testdata;
using Xunit;

namespace NuGetUtils.CLI.Tests.Commands
{
    public class SearchCommandTests
    {
        private readonly AutoMocker autoMocker;

        public SearchCommandTests()
        {
            this.autoMocker = new AutoMocker();
        }

        [Theory]
        [ClassData(typeof(SearchCommandTestdata))]
        public async Task ShouldInvokeAsync(string apiKey, string packageId, bool? preRelease, bool skipLatestStable, bool skipLatestPreRelease)
        {
            // Arrange
            var preReleaseParameter = preRelease != null ? $"--pre {preRelease}" : "";
            var skipLatestStableParameter = skipLatestStable ? "--skip-latest-stable" : "";
            var skipLatestPreReleaseParameter = skipLatestPreRelease ? "--skip-latest-pre" : "";

            var nugetClientMock = this.autoMocker.GetMock<INuGetClient>();
            nugetClientMock.Setup(n => n.SearchAsync(packageId, preRelease, skipLatestStable, skipLatestPreRelease))
                .ReturnsAsync((string id, bool pre, bool sls, bool slpr) => SearchResults.GetTestSearchResult_NuGetUtils_Testdata1());

            var searchCommand = this.autoMocker.CreateInstance<SearchCommand>();

            // Act
            await searchCommand.InvokeAsync($"--api-key {apiKey} --package {packageId} {preReleaseParameter} {skipLatestStableParameter} {skipLatestPreReleaseParameter}");

            // Assert
            nugetClientMock.Verify(n => n.SearchAsync(packageId, preRelease, skipLatestStable, skipLatestPreRelease), Times.Once);
        }

        public class SearchCommandTestdata : TheoryData<string, string, bool?, bool, bool>
        {
            public SearchCommandTestdata()
            {
                this.Add("apikey_test", "packageid_test", null, false, false);
                this.Add("apikey_test", "packageid_test", null, true, false);
                this.Add("apikey_test", "packageid_test", null, false, true);
                this.Add("apikey_test", "packageid_test", null, true, true);
                
                this.Add("apikey_test", "packageid_test", true, false, false);
                this.Add("apikey_test", "packageid_test", true, true, false);
                this.Add("apikey_test", "packageid_test", true, false, true);
                this.Add("apikey_test", "packageid_test", true, true, true);
                
                this.Add("apikey_test", "packageid_test", false, false, false);
                this.Add("apikey_test", "packageid_test", false, true, false);
                this.Add("apikey_test", "packageid_test", false, false, true);
                this.Add("apikey_test", "packageid_test", false, true, true);
            }
        }
    }
}
