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
    public class UnlistCommandTests
    {
        private readonly AutoMocker autoMocker;

        public UnlistCommandTests()
        {
            this.autoMocker = new AutoMocker();
        }

        [Theory]
        [ClassData(typeof(UnlistCommandTestdata))]
        public async Task ShouldInvokeAsync(string apiKey, string packageId, bool? preRelease, bool confirm)
        {
            // Arrange
            var preReleaseParameter = preRelease != null ? $"--pre {preRelease}" : "";
            var confirmParameter = confirm ? "--confirm" : "";

            var nugetClientMock = this.autoMocker.GetMock<INuGetClient>();
            nugetClientMock.Setup(n => n.SearchAsync(packageId, preRelease))
                .ReturnsAsync((string id, bool pre) => SearchResults.GetTestSearchResult(id, pre));

            var unlistCommand = this.autoMocker.CreateInstance<UnlistCommand>();

            // Act
            await unlistCommand.InvokeAsync($"--api-key {apiKey} --package {packageId} {preReleaseParameter} {confirmParameter}");

            // Assert
            nugetClientMock.Verify(n => n.DeletePackageAsync(apiKey, packageId, It.IsAny<string>()), Times.Exactly(2));
        }

        public class UnlistCommandTestdata : TheoryData<string, string, bool?, bool>
        {
            public UnlistCommandTestdata()
            {
                this.Add("apikey_test", "packageid_test", false, true);
                this.Add("apikey_test", "packageid_test", null, true);
                this.Add("apikey_test", "packageid_test", true, true);
            }
        }
    }
}
