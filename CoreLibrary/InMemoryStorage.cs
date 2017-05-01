using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    [Obsolete("There is no need in this class anymore. In accordance with the Requirements we have to use the file system.")]
    public class InMemoryStorage : IStorage
    {
        private static long _lastId;

        public long Insert(IElement element, IContainer parent)
        {
            element.ParentId = parent.Id;

            element.Id = ++_lastId;

            return element.Id;
        }

        public void Rename(IElement element, string newName)
        {

        }

        public long Update(IElement element)
        {
            return element.Id;
        }

        public void Delete(IElement element)
        {
            throw new NotImplementedException();
        }

        public byte[] GetData(IElement element)
        {
            return null;
        }

        public void SetData(IElement element, byte[] data)
        {
        }

        public List<IElement> GetElements(IElement element)
        {
            return new List<IElement>();
        }

        public IElement GetElement(string id)
        {
            throw new NotImplementedException();
        }

        public long GetPhysicalSize(IElement element)
        {
            return 0;
        }

        public IElement DeepCopy(IElement element, string newName)
        {
            throw new NotImplementedException();

            // Project works only with file system

//            var clone = element.Clone(CopyPrefix);
//            //clone.Name = CopyPrefix + clone.Name;
//            container.Add(clone);
//            return clone;
        }
    }
}
