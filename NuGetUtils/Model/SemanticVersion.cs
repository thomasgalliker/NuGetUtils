using System;
using System.Text.RegularExpressions;

namespace NuGetUtils.Model
{
    /// <summary>
    /// Reference: https://semver.org/
    /// </summary>
    public class SemanticVersion : IComparable<SemanticVersion>
    {
        private static Regex SemanticVersionRegex => new Regex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$", RegexOptions.Compiled);

        /// <summary>
        /// Parses the string and creates a SemanticVersion from it
        /// </summary>
        /// <param name="versionString">The string to parse</param>
        /// <returns>The parsed SemanticVersion, or null if the format is incorrect</returns>
        public static SemanticVersion Parse(string versionString)
        {
            var match = SemanticVersionRegex.Match(versionString);
            if (match == null || match.Success == false)
            {
                throw new ArgumentException($"Semantic version format is invalid: \"{versionString}\". Expected equivalent to \"1.2.3\" or \"1.2.3-alpha.4\". See also: https://semver.org/");
            }

            var semanticVersion = new SemanticVersion();
            semanticVersion.Major = int.Parse(match.Groups["major"].ToString());
            semanticVersion.Minor = int.Parse(match.Groups["minor"].ToString());
            semanticVersion.Patch = int.Parse(match.Groups["patch"].ToString());

            semanticVersion.PreRelease = match.Groups["prerelease"].ToString();
            semanticVersion.BuildMetadata = match.Groups["buildmetadata"].ToString();

            return semanticVersion;
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
        /// The tag indicating the type and version of the prerelease
        /// </summary>
        public string PreRelease { get; set; }

        public bool IsPreRelease => !string.IsNullOrEmpty(this.PreRelease);

        /// <summary>
        /// Additional version metadata
        /// </summary>
        public string BuildMetadata { get; set; }

        public override string ToString()
        {
            string preRelease;
            if (!string.IsNullOrEmpty(this.PreRelease))
            {
                preRelease = "-" + this.PreRelease;
            }
            else
            {
                preRelease = "";
            }

            string buildMetadata;
            if (!string.IsNullOrEmpty(this.BuildMetadata))
            {
                buildMetadata = "+" + this.BuildMetadata;
            }
            else
            {
                buildMetadata = "";
            }

            return $"{this.Major}.{this.Minor}.{this.Patch}{preRelease}{buildMetadata}";
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
            {
                throw new ArgumentException(nameof(other));
            }

            if (this.Major != other.Major)
            {
                return this.Major.CompareTo(other.Major);
            }

            if (this.Minor != other.Minor)
            {
                return this.Minor.CompareTo(other.Minor);
            }

            if (this.Patch != other.Patch)
            {
                return this.Patch.CompareTo(other.Patch);
            }

            if (this.PreRelease != other.PreRelease)
            {
                return this.PreRelease.CompareTo(other.PreRelease);
            }

            if (this.BuildMetadata != other.BuildMetadata)
            {
                return this.BuildMetadata.CompareTo(other.BuildMetadata);
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SemanticVersion;
            return other != null && this.Major == other.Major && this.Minor == other.Minor && this.Patch == other.Patch && this.PreRelease == other.PreRelease && this.BuildMetadata == other.BuildMetadata;
        }

        public override int GetHashCode()
        {
            return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.PreRelease.GetHashCode() ^ this.BuildMetadata.GetHashCode();
        }
    }
}