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
            //if (string.IsNullOrEmpty(name))
            //    return;

            Name = name;
            //Id = Storage.Instance.Insert(this); //TODO: Может быть в Folder.Add() создавать? Но там не получается. Проверить.
        }

        public IElement Rename(string name)
        {
            Name = name;
            Storage.Instance.Update(this);
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
