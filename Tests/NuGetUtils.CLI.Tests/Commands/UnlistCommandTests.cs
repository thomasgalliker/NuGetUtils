using System.CommandLine;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NuGetUtils.CLI.Commands;
using NuGetUtils.Services;
using NuGetUtils.Testdata;
using Xunit;

namespace NuGetUtils.CLI.Tests.Commands
{
    public class UnlistCommandTests
    {
        private readonly AutoMocker autoMocker;

        public UnlistCommandTests()
        {
            this.autoMocker = new AutoMocker();
        }

        [Theory]
        [ClassData(typeof(UnlistCommandTestdata))]
        public async Task ShouldInvokeAsync(string apiKey, string packageId, bool? preRelease, bool skipLatestStable, bool skipLatestPreRelease)
        {
            // Arrange
            var preReleaseParameter = preRelease != null ? $"--pre {preRelease}" : "";
            var confirmParameter = "--confirm";
            var skipLatestStableParameter = skipLatestStable ? "--skip-latest-stable" : "";
            var skipLatestPreReleaseParameter = skipLatestPreRelease ? "--skip-latest-pre" : "";

            var nugetClientMock = this.autoMocker.GetMock<INuGetClient>();
            nugetClientMock.Setup(n => n.SearchAsync(packageId, preRelease, skipLatestStable, skipLatestPreRelease))
                .ReturnsAsync((string id, bool pre, bool sls, bool slpr) => SearchResults.GetTestSearchResult_NuGetUtils_Testdata1());

            var unlistCommand = this.autoMocker.CreateInstance<UnlistCommand>();

            // Act
            await unlistCommand.InvokeAsync($"--api-key {apiKey} --package {packageId} {preReleaseParameter} {confirmParameter} {skipLatestStableParameter} {skipLatestPreReleaseParameter}");

            // Assert
            nugetClientMock.Verify(n => n.SearchAsync(packageId, preRelease, skipLatestStable, skipLatestPreRelease), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.0.1-pre.3"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.0.1-pre.5"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.0.1"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.1.0"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.1.4-pre"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.2.0-pre.4"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.2.0-pre.5"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.2.0"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.3.2-pre"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.3.4"), Times.Once);
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, "2.3.8-pre"), Times.Once);
            nugetClientMock.VerifyNoOtherCalls();
        }

        public class UnlistCommandTestdata : TheoryData<string, string, bool?, bool, bool>
        {
            public UnlistCommandTestdata()
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
