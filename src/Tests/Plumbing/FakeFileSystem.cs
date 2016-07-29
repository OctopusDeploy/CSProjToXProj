using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSProjToXProj.Plumbing;

namespace Tests.Plumbing
{
    public class FakeFileSystem : Dictionary<string, string>, IFileSystem
    {
        public FakeFileSystem() : base(StringComparer.CurrentCultureIgnoreCase)
        {
            
        }
        public Stream OpenRead(string filename)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(this[filename]));
        }

        public bool Exists(string path)
        {
            return ContainsKey(path);
        }

        public void WriteAllText(string path, string contents)
        {
            this[path] = contents;
        }

        public void Delete(string path)
        {
            this.Remove(path);
        }

        public string[] ReadAllLines(string path)
        {
            return this[path].Split(new[] {Environment.NewLine}, StringSplitOptions.None);
        }

        public void WriteAllLines(string path, IEnumerable<string> lines)
        {
            this[path] = string.Join(Environment.NewLine, lines);
        }
    }
}