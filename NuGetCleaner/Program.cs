using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NuGetCleaner.Internal;

namespace NuGetCleaner
{
    class Program
    {
        private const string NugetExe = "nuget.exe";

        static void Main(string[] args)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var version = executingAssembly.GetName().Version.ToString();
            var title = $"Nuget Cleaner {version}";
            Console.Title = title;

            var exeBytes = ResourceLoader.GetEmbeddedResourceByteArray(executingAssembly, NugetExe);
            var nugetExePath = Path.Combine(Path.GetTempPath(), NugetExe);
            if (!File.Exists(nugetExePath))
            {
                Console.WriteLine($"Extracting {NugetExe} to {nugetExePath}...");
                using (var exeFile = new FileStream(nugetExePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    exeFile.Write(exeBytes, 0, exeBytes.Length);
                }
            }

            string packageId;
            string apikey;

            if (args.Any() && args.Length == 2)
            {
                packageId = args[0];
                apikey = args[1];
            }
            else
            {
                var codeBase = executingAssembly.CodeBase;
                var thisExe = Path.GetFileName(codeBase);

                Console.WriteLine($"{title}{Environment.NewLine}" +
                                  $"Usage: {thisExe} <PackageId> <ApiKey>{Environment.NewLine}");

                return;
            }

            var source = "https://www.nuget.org/api/v2/package";
            var listCommand = $"list {packageId} -AllVersions -PreRelease";

#if DEBUG
            Console.WriteLine($"{NugetExe} {listCommand}");
#endif

            var listProcess = new Process
            {
                StartInfo =
                {
                    FileName = nugetExePath,
                    Arguments = listCommand,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            listProcess.Start();

            var packages = new List<Package>();
            while (!listProcess.StandardOutput.EndOfStream)
            {
                var line = listProcess.StandardOutput.ReadLine();
                if (line != null)
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var packageName = parts[0].Trim();
                    var packageVersion = parts[1].Trim();
                    packages.Add(new Package(packageName, packageVersion));
                }
            }
            listProcess.WaitForExit();

            var packagesToDelete = packages.Where(p => p.PackageVersion.Contains("-"))
                .OrderBy(p => p.PackageVersion)
                .ToList();
#if DEBUG
            Console.WriteLine($"Number of packages found: {packagesToDelete.Count}");
#endif

            foreach (var package in packagesToDelete)
            {
                DeletePackage(nugetExePath, source, apikey, package);
            }
        }

        private static void DeletePackage(string nugetExePath, string source, string apiKey, Package package)
        {
            var deleteCommand = $"delete {package.PackageName} {package.PackageVersion} -Source {source} -apikey {apiKey} -NonInteractive";
            Console.WriteLine($"{NugetExe} {deleteCommand}");

            var deleteProcess = new Process
            {
                StartInfo =
                {
                    FileName = nugetExePath,
                    Arguments = deleteCommand,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            deleteProcess.Start();

            while (!deleteProcess.StandardOutput.EndOfStream)
            {
                var line = deleteProcess.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }
            while (!deleteProcess.StandardError.EndOfStream)
            {
                var line = deleteProcess.StandardError.ReadLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(line);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            deleteProcess.WaitForExit();
        }
    }

    internal class Package
    {
        public string PackageName { get; }
        public string PackageVersion { get; }

        public Package(string packageName, string packageVersion)
        {
            PackageName = packageName;
            PackageVersion = packageVersion;
        }
    }
}