using System.Threading.Tasks;
using FluentAssertions;
using NuGetUtils.Services;
using NuGetUtils.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace NuGetUtils.Tests
{
    public class NuGetClientTests
    {
        private readonly TestOutputHelperLogger<NuGetClient> logger;
        private readonly ITestOutputHelper testOutputHelper;
        private static readonly DumpOptions dumpOptions = new DumpOptions
        {
            DumpStyle = DumpStyle.CSharp,
            ExcludeProperties = new[] { "Context", "Url", "Registration", "IconUrl", "LicenseUrl", "ProjectUrl" }
        };

        public NuGetClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<NuGetClient>(testOutputHelper);
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("CrossPlatformLibrary.Market", true)]
        [InlineData("CrossPlatformLibrary.Market", false)]
        [InlineData("CrossPlatformLibrary.Market", null)]
        public async Task ShouldSearchAsync(string packageId, bool? preRelease)
        {
            // Arrange
            var nugetClient = new NuGetClient(this.logger, new NuGetClientConfiguration());

            // Act
            var searchResult = await nugetClient.SearchAsync(packageId, preRelease);

            // Assert
            testOutputHelper.WriteLine(ObjectDumper.Dump(searchResult, dumpOptions));

            searchResult.Should().NotBeNull();
            searchResult.Data.Should().HaveCountGreaterThan(0);
        }
    }
}
