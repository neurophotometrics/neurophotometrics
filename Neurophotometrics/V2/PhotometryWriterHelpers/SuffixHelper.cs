using Bonsai.IO;

using System;
using System.IO;
using System.Linq;

namespace Neurophotometrics.V2.PhotometryWriterHelpers
{
    public static class SuffixHelper
    {
        public static string GetSuffix(string absPath, PathSuffix suffix)
        {
            if (suffix == PathSuffix.FileCount)
                return GetFileCountSuffix(absPath);
            else if (suffix == PathSuffix.Timestamp)
                return GetTimestampSuffix();

            return "";
        }

        public static string AppendSuffix(string absPath, string suffix)
        {
            var directoryInfo = new DirectoryInfo(absPath);
            var extension = directoryInfo.Extension;

            if (extension.Length > 0)
                absPath = absPath.Remove(absPath.Length - extension.Length);
            return absPath + suffix + extension;
        }

        private static string GetFileCountSuffix(string absPath)
        {
            var directoryInfo = new DirectoryInfo(absPath);
            var fileName = directoryInfo.Name;
            var extension = directoryInfo.Extension;
            var searchName = fileName;
            if (extension.Length > 0)
                searchName = fileName.Remove(fileName.Length - extension.Length);

            var directories = directoryInfo.Parent.GetDirectories();
            var numDirectories = directories.Count(dir => dir.Name.Contains(searchName));

            var files = directoryInfo.Parent.GetFiles();
            var numFiles = files.Count(file => file.Name.Contains(searchName));
            var sum = numDirectories + numFiles;

            return sum.ToString();
        }

        private static string GetTimestampSuffix()
        {
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace(':', '_');
        }
    }
}