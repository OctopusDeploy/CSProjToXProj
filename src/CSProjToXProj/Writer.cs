using System;
using System.Collections.Generic;
using System.IO;
using CSProjToXProj.Plumbing;
using System.Linq;
using CSProjToXProj.SourceFiles;

namespace CSProjToXProj
{
    public class Writer
    {
        private readonly IFileSystem _fileSystem;

        public Writer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void WriteXProj(string xprojPath, ProjectMetadata projectMetaData)
        {
            var contents = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""14.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <VisualStudioVersion Condition=""'$(VisualStudioVersion)' == ''"">14.0</VisualStudioVersion>
    <VSToolsPath Condition=""'$(VSToolsPath)' == ''"">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
  </PropertyGroup>
  <Import Project=""$(VSToolsPath)\DotNet\Microsoft.DotNet.Props"" Condition=""'$(VSToolsPath)' != ''"" />
  <PropertyGroup Label=""Globals"">
    <ProjectGuid>{projectMetaData.Guid:D}</ProjectGuid>
    <RootNamespace>{projectMetaData.RootNamespace}</RootNamespace>
    <BaseIntermediateOutputPath Condition=""'$(BaseIntermediateOutputPath)'=='' "">.\obj</BaseIntermediateOutputPath>
    <OutputPath Condition=""'$(OutputPath)'=='' "">.\bin\</OutputPath>
  </PropertyGroup>
  <PropertyGroup>
    <SchemaVersion>2.0</SchemaVersion>
  </PropertyGroup>
  <Import Project=""$(VSToolsPath)\DotNet\Microsoft.DotNet.targets"" Condition=""'$(VSToolsPath)' != ''"" />
</Project>";

            _fileSystem.WriteAllText(xprojPath, contents);
        }

        public void WriteProjectJson(string directory, ProjectMetadata projectMetaData, IReadOnlyList<PackageEntry> packages)
        {
            var allPackages = packages.Concat(projectMetaData.ProjectReferences.Select(r => new PackageEntry(r, "*")));

            var packagesJson = string.Join($",\r\n", allPackages.Select(p => $@"    ""{p.Id}"": ""{p.Version}"""));
            var framework = "net" + projectMetaData.TargetFrameworkVersion.Replace("v", "").Replace(".", "");
            var json = @"{
  ""version"": ""1.0.0-*"",

  ""dependencies"": {
" + packagesJson + @"
  },

  ""frameworks"": {
    """+ framework + @""": {
    }
  },
}";

            _fileSystem.WriteAllText(Path.Combine(directory, "project.json"), json);
        }
    }
}