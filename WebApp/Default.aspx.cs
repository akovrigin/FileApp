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
        public static string GetChildren(int operation, bool isFolder, string path, string option)
        {
            Settings.RelativePath = path = path.Replace(Devider, '\\');

            var folder = new Folder("");

            var file = isFolder ? null : new File("");

            var element = isFolder ? (IElement) folder : (IElement) file;

            switch (operation)
            {
                case 1: // Delete
                    element.Delete();
                    //Settings.RelativePath = path.Replace(Devider, '\\');
                    break;
                case 2:
                    //TODO: Оптимизировать этот кусок кода
                    var last = path.Split('\\').Last();
                    Settings.RelativePath = path.Substring(0, path.Length - last.Length);
                    folder = new Folder(last);
                    file = isFolder ? null : new File(last);

                    element = isFolder ? (IElement)folder : (IElement)file;

                    if (!string.IsNullOrWhiteSpace(option))
                        element.Rename(option);

                    break;
                default:
                // Just refresh
                    break;
            }

            long size = isFolder ? folder.GetSize() : file.GetSize();

            var elements = new
            {
                Meta = size,
                Items = folder.GetChildren().Select(e => new
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