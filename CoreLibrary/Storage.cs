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

    public interface IStorage
    {
        long Insert(IElement element, IContainer parent);
        void Rename(IElement element, string newName);
        long Update(IElement element);
        void Delete(IElement element);
        string GetData(IElement element);
        void SetData(IElement element, string data);
        long GetPhysicalSize(IElement element);
        List<IElement> GetElements(IElement element);
        //TODO: Удалить за ненадобностью? И в классах тоже.
        //IElement GetElement(string path); //TODO: Вероятно надо заменить path на id, чтобы и для БД подходило по смыслу
        IElement DeepCopy(IElement element, string newName);
    }

    public class Storage
    {
        private Storage()
        {
            
        }

        private static StorageType _type = StorageType.InMemory;

        public static StorageType Type
        {
            get
            {
                return _type;
            }
            set
            {
                switch (value)
                {
                    case StorageType.FileSystem:
                        Instance = new FileSystemStorage();
                        break;
                    case StorageType.DataBase:
                        Instance = new DataBaseStorage();
                        break;
                    default:
                        Instance = new InMemoryStorage();
                        break;
                }
                _type = value;
            }
        }

        //TODO: Make thread-save instance
        public static IStorage Instance { get; private set; } = new InMemoryStorage();
    }
}
