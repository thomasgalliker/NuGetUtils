# NuGetCleaner
[![Version](https://img.shields.io/github/release/thomasgalliker/NuGetCleaner.svg)](https://github.com/thomasgalliker/NuGetCleaner/releases)

<img src="https://raw.githubusercontent.com/thomasgalliker/NuGetCleaner/develop/icon.png" width="100" height="100" alt="NuGetCleaner" align="right"></img>

NuGetCleaner is a command-line utility to delete/unlist NuGet packages.

### Download
This library is available on github: https://github.com/thomasgalliker/NuGetCleaner/releases


### Usage
```C#
NuGetCleaner.exe <PackageId> <ApiKey>
```

### Known Issues
- Issue 1: Response status code does not indicate success: 403 (The specified API key is invalid, has expired, or does not have permission to access the specified package.).
  - Your NuGet API Key is invalid, has expired or the premission does not allow to unlist the selected package
  - Create a new API Key: Select Scopes "Unlist package", Select Packages: The package(s) you want to be able to unlist with this key.

### License
This project is Copyright &copy; 2019 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
