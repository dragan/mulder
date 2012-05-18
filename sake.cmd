@echo off
cd %~dp0

set EnableNuGetPackageRestore=true
".nuget\NuGet.exe" install Sake -Version 0.1.3 -o packages
"packages\Sake.0.1.3\tools\Sake.exe" -f Sakefile.shade -I packages\Sake.0.1.3\tools\Shared %*
