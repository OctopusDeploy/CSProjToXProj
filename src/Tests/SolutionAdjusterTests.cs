using System;
using CSProjToXProj;
using FluentAssertions;
using NUnit.Framework;
using Tests.Plumbing;

namespace Tests
{
    public class SolutionAdjusterTests
    {
        private const string Before = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.25420.1
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""src"", ""src"", ""{7B75C713-EA67-479A-80E7-9150DB5BF60C}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""CSProjToXProj"", ""src\CSProjToXProj\CSProjToXProj.csproj"", ""{84AA132C-5097-4F1B-9161-6B91D06ADB9C}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Tests"", ""src\Tests\Tests.csproj"", ""{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Release|Any CPU.Build.0 = Release|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C} = {7B75C713-EA67-479A-80E7-9150DB5BF60C}
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C} = {7B75C713-EA67-479A-80E7-9150DB5BF60C}
	EndGlobalSection
EndGlobal
";

        private const string After = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 14
VisualStudioVersion = 14.0.25420.1
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""src"", ""src"", ""{7B75C713-EA67-479A-80E7-9150DB5BF60C}""
EndProject
Project(""{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}"") = ""CSProjToXProj"", ""src\CSProjToXProj\CSProjToXProj.xproj"", ""{84AA132C-5097-4F1B-9161-6B91D06ADB9C}""
EndProject
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""Tests"", ""src\Tests\Tests.csproj"", ""{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C}.Release|Any CPU.Build.0 = Release|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		{84AA132C-5097-4F1B-9161-6B91D06ADB9C} = {7B75C713-EA67-479A-80E7-9150DB5BF60C}
		{F87A0BA7-5550-4FBF-BDF2-D4B1E57F387C} = {7B75C713-EA67-479A-80E7-9150DB5BF60C}
	EndGlobalSection
EndGlobal
";


        [Test]
        public void SolutionIsAdjustedCorrectly()
        {
            const string slnFile = @"x:\foo\bar.sln";
            var guid = Guid.Parse("84AA132C-5097-4F1B-9161-6B91D06ADB9C");
            var fs = new FakeFileSystem
            {
                { slnFile, Before}
            };

            new SolutionAdjuster(fs).Adjust(slnFile, guid);

            fs[slnFile].Should().Be(After);
        }



    }
}