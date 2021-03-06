﻿using System;
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
            Storage.Instance.Insert(element, this);

            GetChildren().Add(element);

            return this;
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
                _elements.ForEach(e => Storage.Instance.Insert(e, this));
            }

            return _elements;

        }

        public override long GetSize()
        {
            return GetChildren().Sum(e => e.GetSize());
        }

        public override IElement Clone(string prefix)
        {
            var clone = new Folder(prefix + Name);

            foreach (var element in GetChildren())
            {
                var elClone = element.Clone("");
                clone.Add(elClone);
            }

            return clone;
        }
    }
}
