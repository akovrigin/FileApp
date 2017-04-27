using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary;
using Xunit;

namespace UnitTests
{
    public class ElementTest
    {
        [Fact]
        public void FileVisitor_SetEditVisitorAndExecuteTwice_IsModifiedAlwaysTrue()
        {
            var file = new File("test.txt", "xxx");

            var visitor = new EditVisitor();

            file.Accept(visitor);

            Assert.True(visitor.IsModified);

            file.Accept(visitor);

            Assert.True(visitor.IsModified);
        }

        [Fact]
        public void FolderVisitor_SetEditVisitorAndExecuteTwice_IsModifiedInverted()
        {
            var file = new Folder("test");

            var visitor = new EditVisitor();

            file.Accept(visitor);

            Assert.True(visitor.IsModified);

            file.Accept(visitor);

            Assert.False(visitor.IsModified);
        }
    }
}
