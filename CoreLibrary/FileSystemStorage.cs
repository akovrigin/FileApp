﻿using System;
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
            element.ParentId = parent.Id;
            element.Id = ++_lastId;

            if (!_relativePath.ContainsKey(element.Id))
                _relativePath.Add(element.Id, GetRelation(parent.Id) + parent.Name + "\\");

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

            return element.Id;
        }

        //TODO: Как-то нелогично, команда "переименовать", но передается oldName, а не newName
        public void Rename(IElement element, string oldName)
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
                    dirInfo.Delete();
            }
        }

        public string GetData(IElement element)
        {
            var path = GetPath(element.Id) + element.Name;
            var fi = new FileInfo(path);

            using (var sr = fi.OpenText())
                return sr.ReadToEnd();
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

            var dirList = dir.EnumerateDirectories().Select(dirInfo => new Folder(dirInfo.Name));

            var fileList = dir.EnumerateFiles().Select(fileInfo => new File(fileInfo.Name, null) {Size = fileInfo.Length});

            list.AddRange(dirList);
            list.AddRange(fileList);

            return list;
        }
    }
}