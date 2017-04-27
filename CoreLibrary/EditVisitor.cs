using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary
{
    public class EditVisitor : IVisitor
    {
        public bool IsModified;

        private void VisitFile(File file)
        {
            var data = file.GetData();

            // Do something with data
            IsModified = true;

            file.SetData(data);
        }

        private void VisitFolder(Folder folder)
        {
            // Do something with data
            IsModified = !IsModified; // Just for testings
        }

        public void Visit(IElement element)
        {
            if (element is File)
                VisitFile((File) element);
            else if (element is Folder)
                VisitFolder((Folder)element);
        }
    }
}