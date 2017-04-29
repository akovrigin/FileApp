using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreLibrary;
using Newtonsoft.Json;

namespace WebApp
{
    public partial class _Default : Page
    {
        private const char Devider = '|';

        public _Default()
        {
            Storage.Type = StorageType.FileSystem;
            Settings.MainFolder = ConfigurationManager.AppSettings["MainFolder"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MainFolder.Value = Settings.MainFolder;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetChildren(bool isFolder, string path)
        {
            Settings.RelativePath = path.Replace(Devider, '\\');

            var mainFolder = new Folder("");

            long size = isFolder ? mainFolder.GetSize() : new File("").GetSize();

            var elements = new
            {
                Meta = size,
                Items = mainFolder.GetChildren().Select(e => new
                {
                    e.Id,
                    e.Name,
                    IsFolder = e is IContainer,
                })
            };

            return JsonConvert.SerializeObject(elements);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetMetaData(string path)
        {
            var name = path.Split(Devider).Last();

            Settings.RelativePath = path.Substring(0, path.Length - name.Length - 1).Replace(Devider, '\\'); ;

            var folder = new Folder(name);

            var size = folder.GetSize();

            return size.ToString();
        }
    }
}