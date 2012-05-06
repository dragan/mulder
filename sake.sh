#!/usr/bin/env bash

export EnableNuGetPackageRestore="true"
mono --runtime=v4.0 ".nuget/NuGet.exe" install Sake -Version 0.1.0-alpha-4 -o packages
mono packages/Sake.0.1.0-alpha-4/tools/Sake.exe -f Sakefile.shade -I packages/Sake.0.1.0-alpha-4/tools/Shared "$@"