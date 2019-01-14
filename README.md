# NuGetCleaner
[![Version](https://img.shields.io/github/release/thomasgalliker/NuGetCleaner.svg)](https://github.com/thomasgalliker/NuGetCleaner/releases)

<img src="https://raw.githubusercontent.com/thomasgalliker/NuGetCleaner/develop/icon.png" width="100" height="100" alt="NuGetCleaner" align="right"></img>

NuGetCleaner is a command-line utility to delete/unlist NuGet packages.

### Download
This library is available on github: https://github.com/thomasgalliker/NuGetCleaner/releases


### Usage
Open a new Command Prompt and start NuGetCleaner.exe with the parameters <PackageId> where you put the ID of your NuGet package and the <ApiKey> which is used for authorization purposes. Make sure the API Key has appropriate rights to "Unlist packages" for the selected NuGet package.
```C#
NuGetCleaner.exe <PackageId> <ApiKey>
```

**Example:**
The following command attempts to unlist all pre-release Versions of "EnumUtils" NuGet package:
```C#
NuGetCleaner.exe EnumUtils apikeyapikeyapikeyapikeyapikeyapikeyapikey
```
Console Output:
```C#
nuget.exe delete EnumUtils 1.0.0-pre1 -Source https://www.nuget.org/api/v2/package -apikey apikeyapikeyapikeyapikeyapikeyapikeyapikey -NonInteractive
WARNING: Deleting EnumUtils 1.0.0-pre1 from the 'https://www.nuget.org/api/v2/package'.
  DELETE https://www.nuget.org/api/v2/package/EnumUtils/1.0.0-pre1
  OK https://www.nuget.org/api/v2/package/EnumUtils/1.0.0-pre1 1271ms
EnumUtils 1.0.0-pre1 was deleted successfully.
nuget.exe delete EnumUtils 1.0.0-pre2 -Source https://www.nuget.org/api/v2/package -apikey apikeyapikeyapikeyapikeyapikeyapikeyapikey -NonInteractive
WARNING: Deleting EnumUtils 1.0.0-pre2 from the 'https://www.nuget.org/api/v2/package'.
  DELETE https://www.nuget.org/api/v2/package/EnumUtils/1.0.0-pre2
  OK https://www.nuget.org/api/v2/package/EnumUtils/1.0.0-pre2 1103ms
EnumUtils 1.0.0-pre2 was deleted successfully.
```

### Known Issues
- Issue 1: Response status code does not indicate success: 403 (The specified API key is invalid, has expired, or does not have permission to access the specified package.).
  - Your NuGet API Key is invalid, has expired or the premission does not allow to unlist the selected package
  - Create a new API Key: Select Scopes "Unlist package", Select Packages: The package(s) you want to be able to unlist with this key.

### License
This project is Copyright &copy; 2019 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
