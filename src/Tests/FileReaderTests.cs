using System;
using CSProjToXProj.SourceFiles;
using FluentAssertions;
using NUnit.Framework;
using Tests.Plumbing;

namespace Tests
{
    [TestFixture]
    public class FileReaderTests
    {
        [Test]
        public void TargetFrameworkIsRead()
        {
            ExecuteReadCsProj().TargetFrameworkVersion.Should().Be("v4.5");
        }

        [Test]
        public void RootNamespaceIsRead()
        {
            ExecuteReadCsProj().RootNamespace.Should().Be("MyProject.Namespace");
        }

        [Test]
        public void GuidIsRead()
        {
            ExecuteReadCsProj().Guid.Should().Be(Guid.Parse("C54911F0-365C-4F52-8634-A17F7A8815AC"));
        }

        [Test]
        public void ProjectReferencesAreRead()
        {
            ExecuteReadCsProj().ProjectReferences.Should().BeEquivalentTo("MyProj.Lib");
        }

        [Test]
        public void FrameworkReferencesAreRead()
        {
            ExecuteReadCsProj().FrameworkReferences.Should().BeEquivalentTo("System.Data");
        }

        [Test]
        public void OutputTypeIsRead()
        {
            ExecuteReadCsProj().OutputType.Should().BeEquivalentTo("Exe");
        }

        private ProjectMetadata ExecuteReadCsProj()
        {
            var projFileName = @"x:\foo\my.csproj";
            var fs = new FakeFileSystem()
            {
                { projFileName, CSProjContents }
            };
            return new FileReader(fs).ReadCSProj(projFileName);
        }

        [Test]
        public void PackagesConfigFileCanBeRead()
        {
            const string packagesFile = @"x:\foo\packages.config";
            var fs = new FakeFileSystem()
            {
                { packagesFile, PackagesConfigContents }
            };
            var results = new FileReader(fs).ReadPackagesConfig(packagesFile);
            results.Should()
                .Contain(p => p.Id == "BouncyCastle" && p.Version == "1.7.0")
                .And
                .Contain(p => p.Id == "GitVersionTask" && p.Version == "3.5.4");
        }


        [Test]
        public void NoPackagesConfigFileReturnsEmptyList()
        {
            var results = new FileReader(new FakeFileSystem()).ReadPackagesConfig(@"x:\foo\my.csproj");
            results.Should().BeEmpty();
        }

        private const string CSProjContents =
        #region Mucho XML
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project DefaultTargets=""Build"" ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">x86</Platform>
    <ProductVersion>12.0.0</ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{C54911F0-365C-4F52-8634-A17F7A8815AC}</ProjectGuid>
    <OutputType>Exe</OutputType>
    <RootNamespace>MyProject.Namespace</RootNamespace>
    <AssemblyName>MyProject</AssemblyName>
    <SolutionDir Condition=""$(SolutionDir) == '' Or $(SolutionDir) == '*Undefined*'"">..\</SolutionDir>
    <RestorePackages>true</RestorePackages>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <TargetFrameworkProfile />
    <NuGetPackageImportStamp>
    </NuGetPackageImportStamp>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|x86' "">
    <DebugSymbols>True</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>False</Optimize>
    <OutputPath>bin\Debug</OutputPath>
    <DefineConstants>DEBUG;</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <Externalconsole>True</Externalconsole>
    <PlatformTarget>x86</PlatformTarget>
    <Prefer32Bit>false</Prefer32Bit>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|x86' "">
    <DebugType>none</DebugType>
    <Optimize>True</Optimize>
    <OutputPath>bin\Release</OutputPath>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <Externalconsole>True</Externalconsole>
    <PlatformTarget>x86</PlatformTarget>
    <Prefer32Bit>false</Prefer32Bit>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""BouncyCastle.Crypto"">
      <HintPath>..\packages\BouncyCastle.1.7.0\lib\Net40-Client\BouncyCastle.Crypto.dll</HintPath>
    </Reference>
    <Reference Include=""System"" />
    <Reference Include=""System.Data"" />
    <ProjectReference Include=""..\Libs\MyProj.Lib\MyProjLib.csproj"">
      <Project>{FAF9A6DD-C669-4F5B-88AF-8AB2B8F3B7C4}</Project>
      <Name>Lib.Core</Name>
    </ProjectReference>
  </ItemGroup>
  <ItemGroup>
    <Compile Include=""..\Solution Items\SolutionInfo.cs"">
      <Link>Properties\SolutionInfo.cs</Link>
    </Compile>
    <Compile Include=""..\Solution Items\VersionInfo.cs"">
      <Link>Properties\VersionInfo.cs</Link>
    </Compile>
    <Compile Include=""CertificateGenerator.cs"" />
    <Compile Include=""Program.cs"" />
    <Compile Include=""Properties\AssemblyInfo.cs"" />
  </ItemGroup>
  <ItemGroup>
    <None Include=""app.config"" />
    <None Include=""packages.config"" />
  </ItemGroup>
  <Import Project=""$(MSBuildBinPath)\Microsoft.CSharp.targets"" />
  <Import Project=""$(SolutionDir)\.nuget\nuget.targets"" />
  <Import Project=""..\packages\GitVersionTask.3.5.4\build\dotnet\GitVersionTask.targets"" Condition=""Exists('..\packages\GitVersionTask.3.5.4\build\dotnet\GitVersionTask.targets')"" />
  <Target Name=""EnsureNuGetPackageBuildImports"" BeforeTargets=""PrepareForBuild"">
    <PropertyGroup>
      <ErrorText>This project references NuGet package(s) that are missing on this computer. Use NuGet Package Restore to download them.  For more information, see http://go.microsoft.com/fwlink/?LinkID=322105. The missing file is {0}.</ErrorText>
    </PropertyGroup>
    <Error Condition=""!Exists('..\packages\GitVersionTask.3.5.4\build\dotnet\GitVersionTask.targets')"" Text=""$([System.String]::Format('$(ErrorText)', '..\packages\GitVersionTask.3.5.4\build\dotnet\GitVersionTask.targets'))"" />
  </Target>
</Project>";
#endregion

        private const string PackagesConfigContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
<packages>
  <package id = ""BouncyCastle"" version=""1.7.0"" targetFramework=""net4"" />
  <package id = ""GitVersionTask"" version=""3.5.4"" targetFramework=""net45"" developmentDependency=""true"" />
</packages>";
    }
}
