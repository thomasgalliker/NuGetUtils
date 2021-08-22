using System;
using FluentAssertions;
using NuGetUtils.Model;
using Xunit;

namespace NuGetUtils.Tests
{
    public class SemanticVersionTests
    {
        [Theory]
        [ClassData(typeof(SemanticVersionParseValidTestdata))]
        public void ShouldParse_Valid(string versionString)
        {
            // Act
            var semanticVersion = SemanticVersion.Parse(versionString);

            // Assert
            semanticVersion.Should().NotBeNull();
            semanticVersion.ToString().Should().Be(versionString);
        }

        public class SemanticVersionParseValidTestdata : TheoryData<string>
        {
            public SemanticVersionParseValidTestdata()
            {
                this.Add("0.0.4");
                this.Add("1.2.3");
                this.Add("10.20.30");
                this.Add("1.1.2-prerelease+meta");
                this.Add("1.1.2+meta");
                this.Add("1.1.2+meta-valid");
                this.Add("1.0.0-alpha");
                this.Add("1.0.0-beta");
                this.Add("1.0.0-alpha.beta");
                this.Add("1.0.0-alpha.beta.1");
                this.Add("1.0.0-alpha.1");
                this.Add("1.0.0-alpha0.valid");
                this.Add("1.0.0-alpha.0valid");
                this.Add("1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay");
                this.Add("1.0.0-rc.1+build.1");
                this.Add("2.0.0-rc.1+build.123");
                this.Add("1.2.3-beta");
                this.Add("10.2.3-DEV-SNAPSHOT");
                this.Add("1.2.3-SNAPSHOT-123");
                this.Add("1.0.0");
                this.Add("2.0.0");
                this.Add("1.1.7");
                this.Add("2.0.0+build.1848");
                this.Add("2.0.1-alpha.1227");
                this.Add("1.0.0-alpha+beta");
                this.Add("1.2.3----RC-SNAPSHOT.12.9.1--.12+788");
                this.Add("1.2.3----R-S.12.9.1--.12+meta");
                this.Add("1.2.3----RC-SNAPSHOT.12.9.1--.12");
                this.Add("1.0.0+0.build.1-rc.10000aaa-kk-0.1");
                this.Add("1.0.0-0A.is.legal");
            }
        }

        [Theory]
        [ClassData(typeof(SemanticVersionParseInvalidTestdata))]
        public void ShouldParse_Invalid(string versionString)
        {
            // Act
            Action action = () => SemanticVersion.Parse(versionString);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        public class SemanticVersionParseInvalidTestdata : TheoryData<string>
        {
            public SemanticVersionParseInvalidTestdata()
            {
                this.Add(null);
                this.Add("");
                this.Add("1");
                this.Add("1.2");
                this.Add("1.2.3-0123");
                this.Add("1.2.3-0123.0123");
                this.Add("1.1.2+.123");
                this.Add("1.2.3.4");
                this.Add("+invalid");
                this.Add("-invalid");
                this.Add("-invalid+invalid");
                this.Add("-invalid.01");
                this.Add("alpha");
                this.Add("alpha.beta");
                this.Add("alpha.beta.1");
                this.Add("alpha.1");
                this.Add("alpha+beta");
                this.Add("alpha_beta");
                this.Add("alpha.");
                this.Add("alpha..beta");
                this.Add("1.0.0-alpha_beta");
                this.Add("-alpha.");
                this.Add("1.0.0-alpha..1.0.0-alpha..1");
                this.Add("1.0.0-alpha...1");
                this.Add("1.0.0-alpha....1");
                this.Add("1.0.0-alpha.....1");
                this.Add("1.0.0-alpha......1");
                this.Add("1.0.0-alpha.......1");
                this.Add("01.1.1");
                this.Add("1.01.1");
                this.Add("1.1.01");
                this.Add("1.2");
                this.Add("1.2.3.DEV");
                this.Add("1.2-SNAPSHOT");
                this.Add("1.2.31.2.3----RC-SNAPSHOT.12.09.1--..12+788");
                this.Add("1.2-RC-SNAPSHOT");
                this.Add("-1.0.3-gamma+b7718");
                this.Add("+justmeta");
                this.Add("9.8.7+meta+meta");
                this.Add("9.8.7-whatever+meta+meta");
                this.Add("99999999999999999999999.999999999999999999.99999999999999999----RC-SNAPSHOT.12.09.1--------------------------------..12");
                this.Add("2.0.19008.2");
                this.Add("2.0.19008.2-pre");
            }
        }
    }
}
