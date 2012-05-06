@echo off
cd %~dp0

set EnableNuGetPackageRestore=true
".nuget\NuGet.exe" install Sake -Version 0.1.0-alpha-4 -o packages
"packages\Sake.0.1.0-alpha-4\tools\Sake.exe" -f Sakefile.shade -I packages\Sake.0.1.0-alpha-4\tools\Shared %*
