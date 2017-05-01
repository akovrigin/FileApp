using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class FileSystemStorage : IStorage
    {
        private static long _lastId;

        private readonly Dictionary<long, string> _relativePath = new Dictionary<long, string>();

        private string GetRelation(long id)
        {
            return _relativePath.ContainsKey(id) ? _relativePath[id] : "";
        }

        private string GetPath(long id)
        {
            return Settings.FullPath + GetRelation(id);
        }

        public long Insert(IElement element, IContainer parent)
        {
            //TODO: There is no sense for file system in Id. Have to move it in DBStorage
            element.ParentId = parent.Id;
            element.Id = ++_lastId;

            if (!_relativePath.ContainsKey(element.Id))
                _relativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + Path.DirectorySeparatorChar);

            var path = GetPath(element.Id);

            if (element is File)
            {
                //File will be created in SetData() 
                //because without the data there is no sence to create file
                // var fileInfo = new FileInfo(path + element.Name);
                // if (!fileInfo.Exists)
                //    fileInfo.Create();
            }
            else if (element is Folder)
            {
                var dirInfo = new DirectoryInfo(path + element.Name);
                if (!dirInfo.Exists)
                    dirInfo.Create();
            }

            return element.Id;
        }

        public void Rename(IElement element, string newName)
        {
            var path = GetPath(element.Id);

            var o = path + element.Name;
            var n = path + newName;

            if (element is File)
                new FileInfo(o).MoveTo(n);
            else if (element is Folder)
                Directory.Move(o, n);
        }

        public long Update(IElement element)
        {
            // Maybe we want to update someting in the  future
            return element.Id;
        }

        public void Delete(IElement element)
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
                    Directory.Delete(dirInfo.FullName, true);
                    //dirInfo.Delete(true);
            }
        }

        public byte[] GetData(IElement element)
        {
            var path = GetPath(element.Id) + element.Name;
            return System.IO.File.ReadAllBytes(path);
        }

        public long GetPhysicalSize(IElement element)
        {
            if (element is File)
            {
                var path = GetPath(element.Id) + element.Name;
                var fi = new FileInfo(path);

                return fi.Exists ? fi.Length : 0;
            }

            return 0;
        }

        public void SetData(IElement element, byte[] data)
        {
            if (data == null)
                return;

            var path = GetPath(element.Id) + element.Name;

            using (var fs = new FileInfo(path).Create())
            {
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }

        public List<IElement> GetElements(IElement element)
        {
            var list = new List<IElement>();

            var dir = new DirectoryInfo(Settings.FullPath + GetRelation(element.Id) + element.Name);

            if (!dir.Exists)
                return list;

            try
            {
                var dirList = dir.EnumerateDirectories().Select(dirInfo => new Folder(dirInfo.Name));

                var fileList = dir.EnumerateFiles().Select(fileInfo => new File(fileInfo.Name) { Size = fileInfo.Length });

                list.AddRange(dirList);
                list.AddRange(fileList);
            }
            catch (Exception e)
            {
                // If some problems with the folder, then we don't have to get it's elements
            }

            return list;
        }

        public IElement DeepCopy(IElement element, string newName)
        {
            var src = Settings.FullPath + element.Name;
            var dest = Settings.FullPath + newName;

            if (element is File)
            {
                var fi = new FileInfo(src);
                System.IO.File.Copy(fi.FullName, fi.DirectoryName + Path.DirectorySeparatorChar + newName, true);
            }
            else
            {
                _DeepCopy(src, dest);
            }

            return element;
        }

        private void _DeepCopy(string src, string dest)
        {
            if (dest[dest.Length - 1] != Path.DirectorySeparatorChar)
                dest += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            foreach (var el in Directory.GetFileSystemEntries(src))
            {
                if (Directory.Exists(el))
                    _DeepCopy(el, dest + Path.GetFileName(el));
                else
                    System.IO.File.Copy(el, dest + Path.GetFileName(el), true);
            }
        }
    }
}