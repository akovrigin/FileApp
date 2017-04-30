using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreLibrary;
using Newtonsoft.Json;
using File = CoreLibrary.File;

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

            var last = path.Split('\\').Last();
            Settings.RelativePath = path.Substring(0, path.Length - last.Length);
            var folder = new Folder(last);
            var file = isFolder ? null : new File(last);

            var element = isFolder ? (IElement)folder : (IElement)file;

            switch (operation)
            {
                case 1: // Delete
                    element.Delete();
                    break;
                case 2: // Rename
                    if (!string.IsNullOrWhiteSpace(option))
                        element.Rename(option);
                    break;
                case 3: // Copy
                    var parent = new Folder("");
                    var original = parent.GetChildren().First(c => c.Name == last);
                    ((ICopiable)original).Copy(parent);
                    break;
                case 4: // New
                    //new Folder("").Add(new Folder(option));
                    folder.Add(new Folder(option));
                    break;
                case 6:
                    break;
                default:
                    // Just refresh
                    break;
            }

            var size = isFolder ? folder.GetSize() : file.GetSize();

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

            //TODO: убрать переменную res
            var res = JsonConvert.SerializeObject(elements);
            return res;
        }

        [WebMethod]
        public static void DownloadFile(string path)
        {
            try
            {
                Settings.RelativePath = path = path.Replace(Devider, '\\');

                var last = path.Split('\\').Last();

                Settings.RelativePath = path.Substring(0, path.Length - last.Length);

                var folder = new Folder("");
                var file = new File(last);
                folder.Add(file);

                var fileName = file.Name;
                var data = file.GetData();

                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment;filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.BinaryWrite(data);

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        [WebMethod]
        public static string UploadFile(string path, string fileName, string based64BinaryString)
        {
            var base64 = "base64,";
            var idx = based64BinaryString.IndexOf(base64, StringComparison.Ordinal);
            var str = based64BinaryString.Substring(idx + base64.Length);

            var data = Convert.FromBase64String(str);

            Settings.RelativePath = path = path.Replace(Devider, '\\');

            var last = path.Split('\\').Last();

            Settings.RelativePath = path.Substring(0, path.Length - last.Length);

            var folder = new Folder(last);
            var file = new File(fileName);
            folder.Add(file);

            file.SetData(data);

            return "ok";
        }
    }
}