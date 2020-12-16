using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RunBot
{
    public static class Settings
    {
        private static string cachedToken;
        public static string Token => cachedToken ?? (cachedToken = File.ReadAllText("token.txt"));

        public const string CommandPrefix = "r.";
    }
}
