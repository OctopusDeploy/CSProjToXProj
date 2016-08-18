[![Build status](https://ci.appveyor.com/api/projects/status/9lvn8n52yq4oc663/branch/master?svg=true)](https://ci.appveyor.com/project/OctopusDeploy/csprojtoxproj/branch/master)

# CSProjToXProj
Converts .csproj files (and packages.config) to .xproj and project.json and optionally updates the solution.

Usage: 
`CSProjToXProj <path_to_search> [/ReplaceExisting] [/Force]`

If `/ReplaceExisting` is specified:
- The project GUID is retained
- The sln file is updated
- The `csproj` and `package.config` files are deleted

Omit this flag if you are intending to have side by side csproj and xproj project file. You will need to rename either the csproj or xproj and then add the xproj to the solution.

`/Force` overwrites existing xproj files
