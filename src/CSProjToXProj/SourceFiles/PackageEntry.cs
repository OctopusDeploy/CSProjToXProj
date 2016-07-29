namespace CSProjToXProj.SourceFiles
{
    public class PackageEntry
    {
        public PackageEntry(string id, string version)
        {
            Id = id;
            Version = version;
        }
        public string Id { get; }
        public string Version { get; }
    }
}