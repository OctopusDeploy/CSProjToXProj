using System;
using System.Collections.Generic;

namespace CSProjToXProj.SourceFiles
{
    public class ProjectMetadata
    {

        public ProjectMetadata(string targetFrameworkVersion, string rootNamespace, Guid guid, string[] projectReferences, string[] frameworkReferences,
            string[] embeddedResources, string outputType, Guid[] projectTypeGuids)
        {
            TargetFrameworkVersion = targetFrameworkVersion;
            RootNamespace = rootNamespace;
            Guid = guid;
            ProjectReferences = projectReferences;
            FrameworkReferences = frameworkReferences;
            EmbeddedResources = embeddedResources;
            OutputType = outputType;
            ProjectTypeGuids = new HashSet<Guid>(projectTypeGuids);
        }

        public string TargetFrameworkVersion { get; }
        public string RootNamespace { get;  }
        public Guid Guid { get;  }
        public IReadOnlyList<string> ProjectReferences { get;  }
        public IReadOnlyList<string> FrameworkReferences { get; }
        public IReadOnlyList<string> EmbeddedResources { get; }
        public string OutputType { get;}
        public ISet<Guid> ProjectTypeGuids { get; }
    }
}