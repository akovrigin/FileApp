﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLibrary
{
    public class DataBaseStorage : IStorage
    {
        public long Insert(IElement element, IContainer parent)
        {
            throw new NotImplementedException();
        }

        public void Rename(IElement element, string newName)
        {
            throw new NotImplementedException();
        }

        public long Update(IElement element)
        {
            throw new NotImplementedException();
        }

        public void Delete(IElement element)
        {
            throw new NotImplementedException();
        }

        public byte[] GetData(IElement element)
        {
            throw new NotImplementedException();
        }

        public void SetData(IElement element, byte[] data)
        {
            throw new NotImplementedException();
        }

        public List<IElement> GetElements(IElement element)
        {
            throw new NotImplementedException();
        }

        public IElement GetElement(string id)
        {
            throw new NotImplementedException();
        }

        public long GetPhysicalSize(IElement element)
        {
            throw new NotImplementedException();
        }

        public IElement DeepCopy(IElement element, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
