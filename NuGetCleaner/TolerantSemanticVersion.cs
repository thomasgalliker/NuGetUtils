// Copyright (c) 2021 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NuGet.Versioning;

namespace SIL.NuGetCleaner
{
	public class TolerantSemanticVersion: SemanticVersion
	{
		public static TolerantSemanticVersion ParseVersion(string versionString)
		{
			if (TryParse(versionString, out var result))
				return new TolerantSemanticVersion(versionString, result);

			// Regex inspired by https://github.com/adamreeve/semver.net
			// Copyright (c) 2016 Adam Reeve
			var looseRegex = new Regex(@"^
					(\d+)                     # major version
					(\.
					(\d+)                     # minor version
					(\.
					(\d+)?                    # patch version
					(\.\d+)*
					(\-?([0-9A-Za-z\-\.]+))?  # pre-release version
					(\+([0-9A-Za-z\-\.]+))?   # build metadata
					\s*)?)?
					$",
				RegexOptions.IgnorePatternWhitespace);
			var match = looseRegex.Match(versionString);
			if (!match.Success)
			{
				Console.WriteLine($"ERROR: version doesn't comply to SemVer standard. Can't parse {versionString}");
				return null;
			}

			var majorString = match.Groups[1].Success ? match.Groups[1].Value : "0";
			var minorString = match.Groups[3].Success ? match.Groups[3].Value : "0";
			var patchString = match.Groups[5].Success ? match.Groups[5].Value : "0";
			var preRelease = match.Groups[8].Success? match.Groups[8].Value : "";
			if (int.TryParse(majorString, out var major) &&
				int.TryParse(minorString, out var minor) &&
				int.TryParse(patchString, out var patch))
			{
				return new TolerantSemanticVersion(versionString, major, minor, patch, preRelease);
			}

			Console.WriteLine($"ERROR: version doesn't comply to SemVer standard. Can't parse {versionString}");
			return null;
		}

		private readonly string _originalVersion;

		public TolerantSemanticVersion(TolerantSemanticVersion version) : base(version)
		{
			_originalVersion = version._originalVersion;
		}

		private TolerantSemanticVersion(string originalVersion, SemanticVersion version) : base(version)
		{
			_originalVersion = originalVersion;
		}

		public TolerantSemanticVersion(int major, int minor, int patch) : base(major, minor, patch)
		{
		}

		private TolerantSemanticVersion(string originalVersion, int major, int minor, int patch, string releaseLabel)
			: base(major, minor, patch, releaseLabel)
		{
			_originalVersion = originalVersion;
		}

		public TolerantSemanticVersion(int major, int minor, int patch, string releaseLabel) : base(major, minor, patch, releaseLabel)
		{
		}

		public TolerantSemanticVersion(int major, int minor, int patch, string releaseLabel, string metadata) : base(major, minor, patch, releaseLabel, metadata)
		{
		}

		public TolerantSemanticVersion(int major, int minor, int patch, IEnumerable<string> releaseLabels, string metadata) : base(major, minor, patch, releaseLabels, metadata)
		{
		}

		protected TolerantSemanticVersion(Version version, string releaseLabel = null, string metadata = null) : base(version, releaseLabel, metadata)
		{
		}

		protected TolerantSemanticVersion(int major, int minor, int patch, int revision, string releaseLabel, string metadata) : base(major, minor, patch, revision, releaseLabel, metadata)
		{
		}

		protected TolerantSemanticVersion(int major, int minor, int patch, int revision, IEnumerable<string> releaseLabels, string metadata) : base(major, minor, patch, revision, releaseLabels, metadata)
		{
		}

		protected TolerantSemanticVersion(Version version, IEnumerable<string> releaseLabels, string metadata) : base(version, releaseLabels, metadata)
		{
		}

		public override string ToString()
		{
			return _originalVersion;
		}
	}
}