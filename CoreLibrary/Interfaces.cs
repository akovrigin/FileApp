using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public interface IElement
    {
        long Id { get; set; }
        long ParentId { get; set; }
        string Name { get; set; }
        IElement Rename(string name);
        long GetSize();
        void Delete();
        IElement Clone(string newName);
        void Accept(IVisitor visitor);
    }

    public interface ICopiable
    {
        IElement Copy(IContainer container);
    }

    public interface IProcessibility
    {
        object Download(object data);
        object Upload(object data);
    }

    public interface IContainer : IElement
    {
        IContainer Add(IElement element);
        IContainer Remove(IElement element);
        List<IElement> GetChildren();
    }

    public interface IHasData
    {
        string GetData();
        void SetData(string data);
    }

    public interface IVisitor
    {
        void Visit(IElement element);
        //void VisitFile(File file);
        //void VisitFolder(Folder folder);
    }
}
