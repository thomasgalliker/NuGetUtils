// Copyright (c) 2021 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using NUnit.Framework;

namespace SIL.NuGetCleaner.Tests
{
	[TestFixture]
	public class TolerantSemanticVersionTests
	{
		[TestCase("1", 1, 0, 0, false)]
		[TestCase("1.2", 1, 2, 0, false)]
		[TestCase("1.2.3", 1, 2, 3, false)]
		[TestCase("1.2.3.4", 1, 2, 3, false)]
		[TestCase("1.2.3.4.5", 1, 2, 3, false)]
		[TestCase("1.2.3-alpha", 1, 2, 3, true)]
		[TestCase("1.2.3-alpha.1", 1, 2, 3, true)]
		[TestCase("1.2.3-alpha0001", 1, 2, 3, true)]
		[TestCase("1.2.3+1", 1, 2, 3, false)]
		[TestCase("1.2.3-pre1+2", 1, 2, 3, true)]
		[TestCase("1.2.3.4-alpha", 1, 2, 3, true)]
		[TestCase("1.2.3.4-alpha.1", 1, 2, 3, true)]
		[TestCase("1.2.3.4-alpha-1", 1, 2, 3, true)]
		[TestCase("1.2.3alpha", 1, 2, 3, true)]
		public void ParseVersion(string version, int expectedMajor, int expectedMinor, int expectedPatch, bool expectedPreRelease)
		{
			var parsedVersion = TolerantSemanticVersion.ParseVersion(version);
			Assert.That(parsedVersion, Is.Not.Null);
			Assert.That(parsedVersion.Major, Is.EqualTo(expectedMajor), "Major version");
			Assert.That(parsedVersion.Minor, Is.EqualTo(expectedMinor), "Minor version");
			Assert.That(parsedVersion.Patch, Is.EqualTo(expectedPatch), "Patch version");
			Assert.That(parsedVersion.IsPrerelease, Is.EqualTo(expectedPreRelease), "IsPreRelease");
			Assert.That($"{parsedVersion}", Is.EqualTo(version));
		}

		[TestCase("")]
		[TestCase("One")]
		[TestCase("1.Two")]
		[TestCase("1bla")]
		[TestCase("1.2bla")]
		public void ParseVersionIllegalVersion(string version)
		{
			var parsedVersion = TolerantSemanticVersion.ParseVersion(version);
			Assert.That(parsedVersion, Is.Null);
		}
	}
}