// Copyright (c) 2019 SIL International
// This software is licensed under the MIT License (http://opensource.org/licenses/MIT)

using CommandLine;

namespace SIL.NuGetCleaner
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class Options
	{
		[Option("api-key", HelpText = "The NuGet API key.")]
		public string ApiKey { get; set; }

		[Option("dry-run", HelpText = "Lists the pre-release versions to unlist without unlisting them.")]
		public bool DryRun { get; set; }

		[Option("min", HelpText = "Minimum version, not included in the versions to unlist (i.e. min<x<max). "+
			"Defaults to 0.")]
		public string Minimum { get; set; }

		[Option("max", HelpText = "Maximum version, not included in the versions to unlist (i.e. min<x<max). " +
			"Defaults to the latest release. Set to -1 to include pre-release versions that have a " +
			"higher version number than the latest release (i.e. min<x)")]
		public string Maximum { get; set; }

		[Value(0, Required = true, HelpText = "The NuGet package id.", MetaName = "packageId")]
		public string PackageId { get; set; }
	}
}
