using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class InMemoryStorage : IStorage
    {
        private static long _lastId;

        public long Insert(IElement element, IContainer parent)
        {
            element.ParentId = parent.Id;

            element.Id = ++_lastId;

            return element.Id; //TODO: Надо ли возвращаться id? Может для DataBase?
        }

        //TODO: Как-то нелогично, команда "переименовать", но передается oldName, а не newName
        public void Rename(IElement element, string oldName)
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

        public string GetData(IElement element)
        {
            return string.Empty;
        }

        public void SetData(IElement element, string data)
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
            throw new NotImplementedException();
        }
    }
}
