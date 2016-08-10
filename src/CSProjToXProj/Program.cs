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
        private const string ReplaceExistingOption = "/ReplaceExisting";
        private const string ForceOption = "/Force";

        private static readonly HashSet<string> AllOptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ReplaceExistingOption,
            ForceOption
        };

        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return 1;
            }

            var root = args[0];
            var flags = new HashSet<string>(args.Skip(1), StringComparer.OrdinalIgnoreCase);
            if(!flags.All(f => AllOptions.Contains(f)))
            {
                PrintUsage();
                return 1;
            }
            var replaceExisting = flags.Contains(ReplaceExistingOption);
            var force = flags.Contains(ForceOption);


            if (!Directory.Exists(root))
            {
                Console.WriteLine($"The directory {root} does not exist");
                return 2;
            }

            try
            {
                Run(root, replaceExisting, force);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Debugger.Break();
                return 3;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: CSProjToXProj <path_to_search> [/ReplaceExisting]");
            Console.WriteLine();
            Console.WriteLine(
                "if /ReplaceExisting is specified, the project GUID is retained, the sln file is updated and the csproj and package.config are deleted");
            Console.WriteLine(
                "omit this flag if you are intending to have side by side csproj and xproj. You will need to rename either the csproj or xproj and then add the xproj to the solution.");
            Console.WriteLine();
            Console.WriteLine("/Force overwrites existing xproj files");
        }

        public static void Run(string directory, bool replaceExisting, bool force)
        {
            foreach (var csprojPath in Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories))
            {
                RunForCSProjFile(csprojPath, replaceExisting, force);
            }
        }

        private static void RunForCSProjFile(string csprojPath, bool replaceExisting, bool force)
        {
            csprojPath = Path.GetFullPath(csprojPath);
            var fs = new FileSystem();
            var fileReader = new FileReader(fs);
            var directory = Path.GetDirectoryName(csprojPath);
            var packagesPath = Path.Combine(directory, "packages.config");
            var xprojPath = Path.ChangeExtension(csprojPath, "xproj");

            if (!force && File.Exists(xprojPath))
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
            writer.WriteXProj(xprojPath, projectMetaData, replaceExisting);
            Console.WriteLine($"Writing project.json to {directory}");
            writer.WriteProjectJson(directory, projectMetaData, packages);

            if (replaceExisting)
            {
                foreach (var slnFile in FindSlnFiles(directory))
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
        }

        private static IEnumerable<string> FindSlnFiles(string directory)
        {
            var slnFiles = Directory.EnumerateFiles(directory, "*.sln");

            if (Directory.GetDirectoryRoot(directory) == directory)
                return slnFiles;

            return slnFiles.Concat(FindSlnFiles(Path.GetDirectoryName(directory)));
        }
    }
}
