using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using CSProjToXProj.Plumbing;
using CSProjToXProj.SourceFiles;

namespace CSProjToXProj
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CSProjToXProj <csproj_file>");
                return 1;
            }

            var csprojPath = args[0];
            if (!File.Exists(csprojPath))
            {
                Console.WriteLine($"The file {csprojPath} does not exist");
                return 2;
            }

            try
            {
                Run(csprojPath);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debugger.Break();
                return 3;
            }
        }

        public static void Run(string csprojPath)
        {
            csprojPath = Path.GetFullPath(csprojPath);
            var fs = new FileSystem();
            var fileReader = new FileReader(fs);
            var directory = Path.GetDirectoryName(csprojPath);
            var packagesPath = Path.Combine(directory, "packages.config");

            Console.WriteLine($"Reading {csprojPath}");
            var projectMetaData = fileReader.ReadCSProj(csprojPath);
            Console.WriteLine($"Reading {packagesPath}");
            var packages = fileReader.ReadPackagesConfig(packagesPath);

            var writer = new Writer(fs);
            Console.WriteLine($"Writing xproj to {directory}");
            writer.WriteXProj(csprojPath, projectMetaData);
            Console.WriteLine($"Writing project.json to {directory}");
            writer.WriteProjectJson(directory, projectMetaData, packages);

            Console.WriteLine($"Deleting {csprojPath}");
            fs.Delete(csprojPath);
            Console.WriteLine($"Deleting {packagesPath}");
            fs.Delete(packagesPath);
        }
    }
}
