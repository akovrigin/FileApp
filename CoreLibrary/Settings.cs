using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            if (MainFolder != null && !MainFolder.EndsWith( new string(Path.DirectorySeparatorChar, 1)))
                MainFolder += Path.DirectorySeparatorChar;
        }

        private static string _relativePath;
        public static string RelativePath{
            get { return _relativePath; }
            set
            {
                _relativePath = value;

                if (value.StartsWith(new string(Path.DirectorySeparatorChar, 1)))
                    _relativePath = value.Substring(1, value.Length - 1);

                if (!value.EndsWith(new string(Path.DirectorySeparatorChar, 1)))
                    _relativePath += Path.DirectorySeparatorChar;
            }
        }

        public static string FullPath => MainFolder + RelativePath;
    }
}
