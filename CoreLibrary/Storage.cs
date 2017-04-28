using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public enum StorageType
    {
        InMemory,
        FileSystem,
        DataBase
    }
    
    public class Storage
    {
        private Storage()
        {
            
        }

        public static StorageType Type { get; set; } = StorageType.InMemory;

        //TODO: Make thread-save instance
        public static Storage Instance { get; private set; } = new Storage();

        private static long _lastId;

        private readonly Dictionary<long, string> RelativePath = new Dictionary<long, string>();

        public void SetRelation(IElement element, IElement parent)
        {
            element.ParentId = parent.Id;

            if (!RelativePath.ContainsKey(element.Id))
                RelativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + "\\");

            Update(element); // DB
        }

        private string GetRelation(long id)
        {
            return RelativePath.ContainsKey(id) ? RelativePath[id] : "";
        }

        public string GetPath(long id)
        {
            return Settings.FullPath + GetRelation(id);
        }

        public long Insert(IElement element, IElement parent)
        {
            element.ParentId = parent.Id;

            if (Type == StorageType.InMemory)
            {
                element.Id = ++_lastId;
            }
            else if (Type == StorageType.FileSystem)
            {
                element.Id = ++_lastId;

                if (!RelativePath.ContainsKey(element.Id))
                    RelativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + "\\");

                var path = GetPath(element.Id);

                if (element is File)
                {
                    var fileInfo = new FileInfo(path + element.Name);
                    if (!fileInfo.Exists)
                        fileInfo.Create();
                    //TODO: Save data to file
                }
                else if (element is Folder)
                {
                    var dirInfo = new DirectoryInfo(path + element.Name);
                    if (!dirInfo.Exists)
                        dirInfo.Create();
                }
            }
            else if (Type == StorageType.DataBase)
            {
                _lastId = Update(element); // DB
            }

            return element.Id;
        }

        public void Rename(IElement element, string oldName)
        {
            if (Type == StorageType.FileSystem)
            {
                var path = GetPath(element.Id);

                if (element is File)
                {
                    new FileInfo(path + oldName).MoveTo(path + element.Name);
                }
                else if (element is Folder)
                {
                    new DirectoryInfo(path + oldName).MoveTo(path + element.Name);
                }
            }
            else if (Type == StorageType.DataBase)
            {
                Update(element); // DB
            }
        }

        public long Update(IElement element)
        {
            return element.Id; //Update in database
        }

        public void Delete(IElement element)
        {
            if (Type == StorageType.FileSystem)
            {
                var path = GetPath(element.Id);

                if (element is File)
                {
                    var fileInfo = new FileInfo(path + element.Name);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                else if (element is Folder)
                {
                    var dirInfo = new DirectoryInfo(path + element.Name);
                    if (dirInfo.Exists)
                        dirInfo.Delete();
                }
            }
            else if (Type == StorageType.DataBase)
            {
                Update(element); // DB
            }
            else if (Type == StorageType.InMemory)
            {
                throw new NotImplementedException();
            }
        }

        public string GetData(IElement element)
        {
            if (Type == StorageType.FileSystem)
            {
                var path = GetPath(element.Id) + element.Name;
                var fi = new FileInfo(path);

                using (var sr = fi.OpenText())
                    return sr.ReadToEnd();
            }

            return null;
        }

        internal void SetData(IElement element, string data)
        {
            if (data == null)
                return;

            var path = GetPath(element.Id) + element.Name;
            var fi = new FileInfo(path);

            using (var fs = fi.Create())
            {
                var info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info, 0, info.Length);
                fs.Flush();
                fs.Close();
            }
        }

        public List<IElement> GetElements(IElement element)
        {
            var list = new List<IElement>();

            if (Type == StorageType.FileSystem)
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
