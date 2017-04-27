using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    class Storage
    {
        private static int _lastId;

        private static readonly Dictionary<int, object> Data = new Dictionary<int, object>();

        public static int Insert(IElement element)
        {
            return _lastId++;
        }
        public static void Update(IElement element)
        {

        }

        public static void Delete(IElement element)
        {
            if (element is IHasData  && Data.ContainsKey(element.Id))
                Data.Remove(element.Id);
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
    }
}
