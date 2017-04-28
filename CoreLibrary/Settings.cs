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
        }

        public static string RelativePath;

        public static string FullPath => MainFolder + RelativePath;
    }
}
