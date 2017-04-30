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

//        public IElement GetElement(string path)
//        {
//            var dirInfo = new DirectoryInfo(path);
//            if (dirInfo.Exists)
//                return new Folder(path.Split('\\').Last());
//
//            var fileInfo = new FileInfo(path);
//            if (fileInfo.Exists)
//                return new File(path.Split('\\').Last());
//
//            return null;
//        }

        public long Insert(IElement element, IContainer parent)
        {
            element.ParentId = parent.Id;
            element.Id = ++_lastId;

            if (!_relativePath.ContainsKey(element.Id))
                _relativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + "\\");

            var path = GetPath(element.Id);

            if (element is File)
            {
                //TODO: Не всегда файл "отпускается". Понять, как его  отпустить.
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
            //new DirectoryInfo(o).MoveTo(n);
        }

        public long Update(IElement element)
        {
            return element.Id; //TODO: Нужер ли этот метод? Или просто обозначить, что можно что-то изменять у объекта?
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
                    //TODO: Bug: Don't know why, but sometimes recursive parameter doesn't work
                    Directory.Delete(dirInfo.FullName, true);
                //dirInfo.Delete(true);
            }
        }

        public string GetData(IElement element)
        {
            var path = GetPath(element.Id) + element.Name;
            var fi = new FileInfo(path);

            using (var sr = fi.OpenText())
                return sr.ReadToEnd();
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

        public void SetData(IElement element, string data)
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

            var dir = new DirectoryInfo(Settings.FullPath + GetRelation(element.Id) + element.Name);

            if (!dir.Exists)
                return list;

            try
            {
                var dirList = dir.EnumerateDirectories().Select(dirInfo => new Folder(dirInfo.Name));

                var fileList = dir.EnumerateFiles().Select(fileInfo => new File(fileInfo.Name, null) {Size = fileInfo.Length});

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
            string src = Settings.FullPath + element.Name;
            string dest = Settings.FullPath + newName;

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
            //TODO: Path.DirectorySeparatorChar - это должно быть везде, а не только здесь
            if (dest[dest.Length - 1] != Path.DirectorySeparatorChar)
                dest += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            var files = Directory.GetFileSystemEntries(src);

            foreach (string el in files)
            {
                // Sub directories
                if (Directory.Exists(el))
                    _DeepCopy(el, dest + Path.GetFileName(el));
                // Files in directory
                else
                    System.IO.File.Copy(el, dest + Path.GetFileName(el), true);
            }
        }
    }
}