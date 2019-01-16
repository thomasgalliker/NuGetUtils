# NuGetCleaner
[![Version](https://img.shields.io/github/release/thomasgalliker/NuGetCleaner.svg)](https://github.com/thomasgalliker/NuGetCleaner/releases)

NuGetCleaner is a command-line utility to delete/unlist NuGet pre-release packages. In order to minimize the number of alpha and beta releases on NuGet, it's a good practice to clean-up old pre-releases. The aim of NuGetCleaner is to remove all pre-release versions of a certain NuGet package. It is recommended to run this utility *after* the successful release of a stable package version. 

### Download
This zipped executable of NuGetCleaner is available on github releases: https://github.com/thomasgalliker/NuGetCleaner/releases


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
- Issue 2: There is no fine-grained filter to include or exclude certain package versions. For simlicity, this feature is not (yet) available. Feel free to fork this repository and create a Pull Request with your changes.

### License
This project is Copyright &copy; 2019 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
