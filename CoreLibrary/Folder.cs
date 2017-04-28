using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class Folder : Element, IContainer
    {
        private List<IElement> _elements;

        protected string RelativePath;

        public Folder(string name) : base(name)
        {
        }

        public IContainer Add(IElement element)
        {
            Actualize(element);

            GetChildren().Add(element);

            return this;
        }

        private void Actualize(IElement element)
        {
            //Storage.Instance.SetRelation(element, this);
            Storage.Instance.Insert(element, this);
        }

        public IContainer Remove(IElement element)
        {
            _elements?.Remove(element);

            element.Delete();

            return this;
        }

        public List<IElement> GetChildren()
        {
            if (_elements == null)
            {
                _elements = Storage.Instance.GetElements(this);
                _elements.ForEach(Actualize);
            }

            return _elements;

        }

        public override long GetSize()
        {
            return GetChildren().Sum(e => e.GetSize());
        }

        public override IElement Clone()
        {
            var clone = new Folder(Name);

            foreach (var element in GetChildren())
                clone.Add(element.Clone());

            return clone;
        }
    }
}
