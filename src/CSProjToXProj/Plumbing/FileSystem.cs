using System.Collections.Generic;
using System.IO;

namespace CSProjToXProj.Plumbing
{

    public interface IFileSystem
    {
        Stream OpenRead(string path);
        bool Exists(string path);
        void WriteAllText(string path, string contents);
        void Delete(string path);
        string[] ReadAllLines(string path);
        void WriteAllLines(string path, IEnumerable<string> lines);
    }

    public class FileSystem : IFileSystem
    {
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public void WriteAllLines(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
        }
    }

}