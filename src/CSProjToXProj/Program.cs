using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using CSProjToXProj.Plumbing;
using CSProjToXProj.SourceFiles;
using System.Linq;

namespace CSProjToXProj
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CSProjToXProj <path_to_search>");
                return 1;
            }

            var root = args[0];
            if (!Directory.Exists(root))
            {
                Console.WriteLine($"The directory {root} does not exist");
                return 2;
            }

            try
            {
                Run(root);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debugger.Break();
                return 3;
            }
        }

        public static void Run(string directory)
        {
            foreach (var csprojPath in Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories))
            {
                RunForCSProjFile(csprojPath);
            }
        }

        private static void RunForCSProjFile(string csprojPath)
        {
            csprojPath = Path.GetFullPath(csprojPath);
            var fs = new FileSystem();
            var fileReader = new FileReader(fs);
            var directory = Path.GetDirectoryName(csprojPath);
            var packagesPath = Path.Combine(directory, "packages.config");
            var xprojPath = Path.ChangeExtension(csprojPath, "xproj");

            if (File.Exists(xprojPath))
            {
                Console.Error.WriteLine($"{xprojPath} exists, skipping {csprojPath}");
                return;
            }

            Console.WriteLine($"{csprojPath} found");
            Console.WriteLine($"Reading {csprojPath}");
            var projectMetaData = fileReader.ReadCSProj(csprojPath);
            Console.WriteLine($"Reading {packagesPath}");
            var packages = fileReader.ReadPackagesConfig(packagesPath);

            var writer = new Writer(fs);
            Console.WriteLine($"Writing xproj to {directory}");
            writer.WriteXProj(xprojPath, projectMetaData);
            Console.WriteLine($"Writing project.json to {directory}");
            writer.WriteProjectJson(directory, projectMetaData, packages);

            foreach(var slnFile in FindSlnFiles(directory))
            {
                Console.WriteLine($"Adjusting {slnFile}");
                new SolutionAdjuster(fs).Adjust(slnFile, projectMetaData.Guid);
            }

            Console.WriteLine($"Deleting {csprojPath}");
            fs.Delete(csprojPath);
            Console.WriteLine($"Deleting {packagesPath}");
            fs.Delete(packagesPath);
            Console.WriteLine();
        }

        private static IEnumerable<string> FindSlnFiles(string directory)
        {
            var slnFiles = Directory.EnumerateFiles(directory, "*.sln");

            if(Directory.GetDirectoryRoot(directory) == directory)
                return slnFiles;

            return slnFiles.Concat(FindSlnFiles(Path.GetDirectoryName(directory)));
        }
    }
}
