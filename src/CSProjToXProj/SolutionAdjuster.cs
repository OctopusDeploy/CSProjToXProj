using System;
using CSProjToXProj.Plumbing;
using CSProjToXProj.SourceFiles;
using System.Linq;

namespace CSProjToXProj
{
    public class SolutionAdjuster
    {
        private const string CSProjTypeGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        private const string XProjTypeGuid = "8BB2217D-0F2D-49D1-97BC-3654ED321F3B";

        private readonly IFileSystem _fileSystem;




        public SolutionAdjuster(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public void Adjust(string slnFile, Guid projectGuid)
        {
            var projectGuidStr = projectGuid.ToString("D").ToUpper();
            var lines = _fileSystem.ReadAllLines(slnFile)
                .Select(
                    l => l.Contains(CSProjTypeGuid) && l.Contains(projectGuidStr)
                        ? l.Replace(CSProjTypeGuid, XProjTypeGuid).Replace("csproj", "xproj")
                        : l
                );
            _fileSystem.WriteAllLines(slnFile, lines);
        }
    }
}