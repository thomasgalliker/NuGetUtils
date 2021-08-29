using System.CommandLine;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NuGetUtils.CLI.Commands;
using NuGetUtils.Services;
using Xunit;

namespace NuGetUtils.CLI.Tests.Commands
{
    public class RelistCommandTests
    {
        private readonly AutoMocker autoMocker;

        public RelistCommandTests()
        {
            this.autoMocker = new AutoMocker();
        }

        [Theory]
        [ClassData(typeof(RelistCommandTestdata))]
        public async Task ShouldInvokeAsync(string apiKey, string packageId, string versions)
        {
            // Arrange
            var confirmParameter = "--confirm";
            var versionsParameter = versions != null ? $"--versions {versions}" : "";

            var nugetClientMock = this.autoMocker.GetMock<INuGetClient>();
            var unlistCommand = this.autoMocker.CreateInstance<RelistCommand>();

            // Act
            await unlistCommand.InvokeAsync($"--api-key {apiKey} --package {packageId} {confirmParameter} {versionsParameter}");

            // Assert
            nugetClientMock.Verify(n => n.SearchAsync(It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Never);

            if (versions != null)
            {
                var versionsArray = versions.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var version in versionsArray)
                {
                    nugetClientMock.Verify(n => n.RelistPackageAsync(apiKey, packageId, version), Times.Once);

                }
            }

            nugetClientMock.VerifyNoOtherCalls();
        }

        public class RelistCommandTestdata : TheoryData<string, string, string>
        {
            public RelistCommandTestdata()
            {
                this.Add("apikey_test", "packageid_test", null);
                this.Add("apikey_test", "packageid_test", "");
                this.Add("apikey_test", "packageid_test", "1.0.0");
                this.Add("apikey_test", "packageid_test", "1.0.0,2.0.0-alpha");
            }
        }
    }
}
