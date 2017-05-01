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

        public long Id { get; set; }
        public long ParentId { get; set; }

        public string Name { get; set; }

        protected Element(string name)
        {
            Name = name;
        }

        public IElement Rename(string name)
        {
            Storage.Instance.Rename(this, name);
            Name = name;
            return this;
        }

        public virtual long GetSize()
        {
            return 0;
        }

        public void Delete()
        {
            Storage.Instance.Delete(this);
        }

        public abstract IElement Clone(string newName);

        public virtual IElement Copy(IContainer container)
        {
            var newName = CopyPrefix + Name;

            Storage.Instance.DeepCopy(this, newName);

            return this;

            //TODO: Maybe it'll be better to return the copied object instead of the original?
            //return container.GetChildren().First(c => c.Name == newName);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
