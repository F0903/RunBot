using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;

namespace RunBot
{
    public static class PathUtils
    {
        public static string GetOrCreatePath(string path)
        {
            if (path.Contains('/') || path.Contains('\\'))
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
            return path;
        }
    }
}
