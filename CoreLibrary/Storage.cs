using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        byte[] GetData(IElement element);
        void SetData(IElement element, byte[] data);
        long GetPhysicalSize(IElement element);
        List<IElement> GetElements(IElement element);
        IElement DeepCopy(IElement element, string newName);
    }

    public class Storage
    {
        static Storage()
        {
            lock (Locker)
            {
                _type = StorageType.InMemory;
                Instance = Lazy.Value;
            }
        }

        private static StorageType _type;

        public static IStorage Instance { get; private set; }

        private static Lazy<IStorage> Lazy => new Lazy<IStorage>(() =>
        {
            switch (_type)
            {
                case StorageType.FileSystem:
                    return new FileSystemStorage();
                case StorageType.DataBase:
                    return new DataBaseStorage();
                default:
                    return new InMemoryStorage();
            }
        });

        private static readonly object Locker = new object();

        public static StorageType Type
        {
            get
            {
                return _type;
            }
            set
            {
                lock (Locker)
                {
                    if (_type == value)
                        return;

                    _type = value;
                    Instance = Lazy.Value;
                }
            }
        }
    }
}
