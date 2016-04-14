using System.IO;

namespace utils
{
    public static class PathUtils
    {
        public static string GetRelativePath(string from, string to)
        {
            var absFrom = Path.GetFullPath(from);
            var absTo = Path.GetFullPath(to);
            if (Path.GetPathRoot(absFrom) != Path.GetPathRoot(absTo))
            {
                return null;
            }
            var fromDirs = SplitPath(absFrom);
            var toDirs = SplitPath(absTo);
            int firstDifferent = GetFirstDifferent(fromDirs, toDirs);
            string rez = "";
            for (int k = firstDifferent; k < fromDirs.Length; k++)
            {
                rez = Path.Combine(rez, "..");
            }
            for (int k = firstDifferent; k < toDirs.Length; k++)
            {
                rez = Path.Combine(rez, toDirs[k]);
            }
            return rez;
        }

        private static int GetFirstDifferent(string[] fromDirs, string[] toDirs)
        {
            int i = 0;
            for (; i < fromDirs.Length && i < toDirs.Length && fromDirs[i].Equals(toDirs[i]); i++)
            {
            }
            return i;
        }

        public static string[] SplitPath(string path)
        {
            return path.Split(Path.DirectorySeparatorChar);
        }
    }
}
