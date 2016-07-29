namespace Tests.Plumbing
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r", "").Replace("\n", "\r\n");
        }
    }
}