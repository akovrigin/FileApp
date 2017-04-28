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

        public File(string name, object data) : base(name)
        {
            SetData(data);
            Size = CalcSize(data);
            Processing = new Processing();
        }

        public File(string name, object data, IProcessibility processing) : base(name)
        {
            SetData(data);
            Size = CalcSize(data);
            Processing = processing;
        }

        private int CalcSize(object data)
        {
            //TODO: Определить размер фала
            return data?.ToString().Length ?? 0;
        }

        public override long GetSize()
        {
            return Size;
        }

        public object GetData()
        {
            //TODO: Download: this action should stream the file to the user’s browser
            return Storage.GetData(Id);
        }

        public void SetData(object data)
        {
            Storage.SetData(Id, data);
        }

        public override IElement Clone()
        {
            return new File(Name, GetData()) {Size = Size};
        }

        public IProcessibility Processing { private get; set; }

        public object Download()
        {
            return Processing.Download(GetData());
        }

        public void Upload(object data)
        {
            Processing.Upload(data);
        }
    }
}
