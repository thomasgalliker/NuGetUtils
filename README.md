# NuGetUtils
[![Version](https://img.shields.io/nuget/v/NuGetUtils.svg)](https://www.nuget.org/packages/NuGetUtils)  [![Downloads](https://img.shields.io/nuget/dt/NuGetUtils.svg)](https://www.nuget.org/packages/NuGetUtils)

<img src="https://raw.githubusercontent.com/thomasgalliker/NuGetUtils/develop/Images/logo.png" height="100" alt="NuGetUtils" align="right">
NuGetUtils is a collection helpful commands in order to simplify nuget package administration.

### Download and Install NuGetUtils

###### Command-Line Utility
NuGetUtils.CLI is available either as  as .NET Core tool or as [zipped executable](https://github.com/thomasgalliker/NuGetUtils/releases).

    dotnet tool install --global NuGetUtils.CLI

You can use the command-line utility on any system running .NET 5 or newer.

###### .NET Library
This utility is also available as .NET library: https://www.nuget.org/packages/NuGetUtils
Use the following command to install NuGetUtils using NuGet package manager console:

    PM> Install-Package NuGetUtils

You can use this library in any .NET project which is compatible to .NET Standard 2.0 and higher.

### Using NuGetUtils.CLI
The following sections document basic use cases of NuGetUtils.CLI.

```
Usage:
  NuGetUtils.CLI [options] [command]

Options:
  --silent        Silences command output on standard out [default: False]
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  unlist  Unlists NuGet packages
```
 

#### Unlist NuGet Packages
The unlist command allows to bulk unlist a specified nuget package.
```
unlist
  Unlists NuGet packages

Usage:
  NuGetUtils.CLI [options] unlist

Options:
  --api-key <api-key> (REQUIRED)  A valid/unrevoked NuGet API key which has the appropriate privileges to run the
                                  command.
  --package <package> (REQUIRED)  The NuGet package idientifier.
  --pre                           Filter pre-release packages. If true, only pre-release packages are included. If
                                  false, only stable packages are included. If not specified, all (stable and
                                  pre-release) packages are included. [default: False]
  --confirm                       Confirms all user interactions. [default: False]
  --silent                        Silences command output on standard out. [default: False]
  -?, -h, --help                  Show help and usage information
```
**Example:** Unlist all pre-releases of NuGet package "EnumUtils".
```
NuGetUtils.CLI unlist --api-key 12345678 --package EnumUtils --pre --confirm
```

### Links
NuGet API Rate Limits
https://docs.microsoft.com/en-us/nuget/api/rate-limits

### Contribution
Contributors welcome! If you find a bug or you want to propose a new feature, feel free to do so by opening a new issue on github.com.

### License
This project is Copyright &copy; 2021 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
