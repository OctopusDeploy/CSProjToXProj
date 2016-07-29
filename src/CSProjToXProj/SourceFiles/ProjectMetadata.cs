using System;
using System.Collections.Generic;

namespace CSProjToXProj.SourceFiles
{
    public class ProjectMetadata
    {

        public ProjectMetadata(string targetFrameworkVersion, string rootNamespace, Guid guid, string[] projectReferences)
        {
            TargetFrameworkVersion = targetFrameworkVersion;
            RootNamespace = rootNamespace;
            Guid = guid;
            ProjectReferences = projectReferences;
        }
        public string TargetFrameworkVersion { get; }
        public string RootNamespace { get;  }
        public Guid Guid { get; set; }
        public IReadOnlyList<string> ProjectReferences { get; set; }
    }
}