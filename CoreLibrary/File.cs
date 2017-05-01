using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class File : Element, IHasData
    {
        public long Size { get; set; }

        public IProcessibility Processing { private get; set; }

        public File(string name, byte[] data) : base(name)
        {
            Processing = new Processing();
            SetData(data);
        }

        public File(string name) : base(name)
        {
            Processing = new Processing();
            Size = Storage.Instance.GetPhysicalSize(this);
        }

        public File(string name, IProcessibility processing) : base(name)
        {
            Processing = processing;
        }

        public override long GetSize()
        {
            return Size;
        }

        public byte[] GetData()
        {
            var data = Storage.Instance.GetData(this);
            Processing.Download(data);
            return data;
        }

        public void SetData(byte[] data)
        {
            data = Processing.Upload(data);
            Storage.Instance.SetData(this, data);
        }

        public override IElement Clone(string prefix)
        {
            return new File(prefix + Name, GetData()) {Size = Size};
        }
    }
}
