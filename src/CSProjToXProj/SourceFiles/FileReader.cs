using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CSProjToXProj.Plumbing;

namespace CSProjToXProj.SourceFiles
{
    public class FileReader
    {
        private readonly IFileSystem _fileSystem;

        public FileReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ProjectMetadata ReadCSProj(string csprojPath)
        {
            using (var s = _fileSystem.OpenRead(csprojPath))
            {
                var doc = XDocument.Load(s);
                const string ns = "http://schemas.microsoft.com/developer/msbuild/2003";

                var projectReferences = doc.Descendants(XName.Get("ProjectReference", ns))
                    .Select(r => r.Attribute("Include").Value)
                    .Select(p => Path.GetFileName(Path.GetDirectoryName(p)))
                    .ToArray();

                var frameworkReferences = doc.Descendants(XName.Get("Reference", ns))
                    .Where(r => r.Descendants(XName.Get("HintPath", ns)).None())
                    .Select(r => r.Attribute("Include").Value)
                    .Except(new [] {"System"})
                    .ToArray();

                return new ProjectMetadata(
                    targetFrameworkVersion: doc.Descendants(XName.Get("TargetFrameworkVersion", ns)).FirstOrDefault()?.Value,
                    rootNamespace: doc.Descendants(XName.Get("RootNamespace", ns)).FirstOrDefault()?.Value,
                    guid: Guid.Parse(doc.Descendants(XName.Get("ProjectGuid", ns)).FirstOrDefault()?.Value),
                    projectReferences: projectReferences,
                    frameworkReferences: frameworkReferences,
                    outputType: doc.Descendants(XName.Get("OutputType", ns)).FirstOrDefault()?.Value
                );
            }
        }

        public IReadOnlyList<PackageEntry> ReadPackagesConfig(string packagesPath)
        {
            if (!_fileSystem.Exists(packagesPath))
                return new PackageEntry[0];

            using (var s = _fileSystem.OpenRead(packagesPath))
            {
                var doc = XDocument.Load(s);
                return doc.Descendants(XName.Get("package"))
                    .Select(e => new PackageEntry(
                        e.Attribute("id").Value,
                        e.Attribute("version").Value
                    ))
                    .ToArray();
            }
        }

    }
}