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

        public NuGetClientTests(ITestOutputHelper testOutputHelper)
        {
            this.logger = new TestOutputHelperLogger<NuGetClient>(testOutputHelper);
        }

        [Fact]
        public async Task ShouldSearchAsync()
        {
            // Arrange
            var nugetClient = new NuGetClient(this.logger, new NuGetClientConfiguration());

            // Act
            var searchResult = await nugetClient.SearchAsync("EnumUtils", preRelease: true);

            // Assert
            searchResult.Should().NotBeNull();
            searchResult.Data.Should().HaveCountGreaterThan(0);
        }
    }
}
