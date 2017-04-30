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

            //TODO: Нужно, чтобы правильно возвращалось значение
            //return container.GetChildren().First(c => c.Name == newName);

            //TODO: Или переделать под InMemory, или удалить
            var clone = Clone(CopyPrefix);
            //clone.Name = CopyPrefix + clone.Name;
            container.Add(clone);
            return clone;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
