using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class Settings
    {
        public static string MainFolder;

        static Settings()
        {
            MainFolder = ConfigurationManager.AppSettings["MainFolder"];
            if (MainFolder != null && !MainFolder.EndsWith("\\"))
                MainFolder += "\\";
        }

        private static string _relativePath;
        public static string RelativePath{
            get { return _relativePath; }
            set { _relativePath = value.StartsWith("\\") ? value.Substring(1, value.Length - 1) : value; }
        }

        public static string FullPath => MainFolder + RelativePath;
    }
}
