using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class Folder : Element, IContainer
    {
        private readonly List<IElement> _elements = new List<IElement>();

        public Folder(string name) : base(name)
        {
        }

        public IContainer Add(IElement element)
        {
            _elements.Add(element);
            element.ParentId = Id;
            return this;
        }

        public IContainer Remove(IElement element)
        {
            _elements.Remove(element);
            element.Delete();
            return this;
        }

        public List<IElement> GetChildren()
        {
            return _elements;
        }

        public override int GetSize()
        {
            return _elements.Sum(e => e.GetSize());
        }

        public override IElement Clone()
        {
            var clone = new Folder(Name);

            foreach (var element in _elements)
                clone.Add(element.Clone());

            return clone;
        }
    }
}
