using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public abstract class Element : IElement, ICopiable
    {
        protected string CopyPrefix = "Copy of ";

        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string Name { get; set; }

        protected Element(string name)
        {
            Name = name;
            Id = Storage.Insert(this);
        }

        public IElement Rename(string name)
        {
            Name = name;
            Storage.Update(this);
            return this;
        }

        public virtual long GetSize()
        {
            return 0;
        }

        public void Delete()
        {
            Storage.Delete(this);
        }

        public abstract IElement Clone();

        public IElement Copy(IContainer container)
        {
            var clone = Clone();
            clone.Name = CopyPrefix + clone.Name;
            return clone;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
