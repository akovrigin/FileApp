using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public interface IElement
    {
        int Id { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
        IElement Rename(string name);
        int GetSize();
        void Delete();
        IElement Clone();
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
        object GetData();
        void SetData(object data);
    }

    public interface IVisitor
    {
        void Visit(IElement element);
        //void VisitFile(File file);
        //void VisitFolder(Folder folder);
    }
}
