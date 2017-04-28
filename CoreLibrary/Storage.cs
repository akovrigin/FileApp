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
        void Rename(IElement element, string oldName);
        long Update(IElement element);
        void Delete(IElement element);
        string GetData(IElement element);
        void SetData(IElement element, string data);
        List<IElement> GetElements(IElement element);
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
