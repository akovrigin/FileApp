using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class Storage
    {
        private static int _lastId;

        private static readonly Dictionary<long, object> Data = new Dictionary<long, object>();

        private static readonly Dictionary<long, string> RelativePath = new Dictionary<long, string>();

        public static int Insert(IElement element)
        {
            //TODO: Remove in file system
            return _lastId++; // DB
        }

        public static void Update(IElement element)
        {

        }

        public static void SetRelation(IElement element, IElement parent)
        {
            element.ParentId = parent.ParentId;
            RelativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + "\\");
            Update(element); // DB
        }

        private static string GetRelation(long id)
        {
            return RelativePath.ContainsKey(id) ? RelativePath[id] : "";
        }

        public static void Delete(IElement element)
        {
            if (element is IHasData  && Data.ContainsKey(element.Id))
                Data.Remove(element.Id);
            //TODO: Remove in file system
        }

        public static object GetData(int id)
        {
            return Data[id];
        }

        internal static void SetData(int id, object data)
        {
            if (Data.ContainsKey(id))
                Data[id] = data;
            else
                Data.Add(id, data);
        }

        public static List<IElement> GetElements(IElement element)
        {
            var list = new List<IElement>();

            if (Settings.Storage == StorageType.FileSystem)
            {
                var dir = new DirectoryInfo(Settings.FullPath + GetRelation(element.Id) + element.Name);

                var dirList = dir.EnumerateDirectories().Select(dirInfo => new Folder(dirInfo.Name));

                var fileList = dir.EnumerateFiles().Select(fileInfo => new File(fileInfo.Name, null) {Size = fileInfo.Length});

                list.AddRange(dirList);
                list.AddRange(fileList);
            }

            return list;
        }
    }
}
