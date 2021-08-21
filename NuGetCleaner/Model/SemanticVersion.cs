using System;
using System.Text.RegularExpressions;

namespace NuGetCleaner.CLI.Model
{
    public class SemanticVersion : IComparable<SemanticVersion>
    {
        private static Regex ParseRegex => new Regex(@"([0-9]+)\.([0-9]+)\.([0-9]+)(\.([0-9]+))?(\-([0-9A-Za-z]+))?(\+([0-9A-Za-z]+))?", RegexOptions.Compiled);

        /// <summary>
        /// Parses the string and creates a SemanticVersion from it
        /// </summary>
        /// <param name="versionString">The string to parse</param>
        /// <returns>The parsed SemanticVersion, or null if the format is incorrect</returns>
        public static SemanticVersion Parse(string versionString)
        {
            var match = ParseRegex.Match(versionString);
            if (match != null)
            {
                var version = new SemanticVersion();
                version.Major = int.Parse(match.Groups[1].Value);
                version.Minor = int.Parse(match.Groups[2].Value);
                version.Patch = int.Parse(match.Groups[3].Value);

                int build;
                if (int.TryParse(match.Groups[5].Value, out build))
                {
                    version.Build = build;
                }

                version.PreReleaseTag = match.Groups[7].Value;
                version.Metadata = match.Groups[9].Value;

                return version;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The major version.
        /// </summary>
        /// <remarks>
        /// The major version only increments on backwards-incompatible changes.
        /// </remarks>
        public int Major { get; set; }

        /// <summary>
        /// The minor version.
        /// </summary>
        /// <remarks>
        /// The minor version increments on backwards-compatible changes.
        /// </remarks>
        public int Minor { get; set; }

        /// <summary>
        /// The patch version.
        /// </summary>
        /// <remarks>
        /// The patch version increments when changes include only fixes.
        /// </remarks>
        public int Patch { get; set; }

        /// <summary>
        /// The build number.
        /// </summary>
        /// <remarks>
        /// Not strictly part of the semantic version, but part of other versioning methods
        /// </remarks>
        public int? Build { get; set; }

        /// <summary>
        /// The tag indicating the type and version of the prerelease
        /// </summary>
        public string PreReleaseTag { get; set; }

        public bool IsPreRelease => !string.IsNullOrEmpty(this.PreReleaseTag);
        /// <summary>
        /// Additional version metadata
        /// </summary>
        public string Metadata { get; set; }

        public override string ToString()
        {
            string buildPart;
            if (this.Build.HasValue)
            {
                buildPart = "." + this.Build.Value.ToString();
            }
            else
            {
                buildPart = "";
            }

            string prerelease;
            if (!string.IsNullOrEmpty(this.PreReleaseTag))
            {
                prerelease = "-" + this.PreReleaseTag;
            }
            else
            {
                prerelease = "";
            }

            string metadata;
            if (!string.IsNullOrEmpty(this.Metadata))
            {
                metadata = "+" + this.Metadata;
            }
            else
            {
                metadata = "";
            }

            return $"{this.Major.ToString()}.{this.Minor.ToString()}.{this.Patch.ToString()}{buildPart}{prerelease}{metadata}";
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj"/>.
        /// Zero
        /// This instance is equal to <paramref name="obj"/>.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <remarks>Original implementation by yadyn: https://gist.github.com/yadyn/959467 (Retrieved 2017-03-10)</remarks>
        public int CompareTo(SemanticVersion other)
        {
            if (other == null)
                throw new ArgumentException(nameof(other));

            if (this.Major != other.Major)
                return this.Major.CompareTo(other.Major);
            if (this.Minor != other.Minor)
                return this.Minor.CompareTo(other.Minor);
            if (this.Patch != other.Patch)
                return this.Patch.CompareTo(other.Patch);
            if (this.PreReleaseTag != other.PreReleaseTag)
                return this.PreReleaseTag.CompareTo(other.PreReleaseTag);
            if (this.Metadata != other.Metadata)
                return this.Metadata.CompareTo(other.Metadata);

            return 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SemanticVersion;
            return other != null && this.Major == other.Major && this.Minor == other.Minor && this.Patch == other.Patch && this.Build == other.Build && this.PreReleaseTag == other.PreReleaseTag && this.Metadata == other.Metadata;
        }

        public override int GetHashCode()
        {
            return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.Build.GetHashCode() ^ this.PreReleaseTag.GetHashCode() ^ this.Metadata.GetHashCode();
        }
    }
}