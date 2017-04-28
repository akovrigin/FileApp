using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary;
using Xunit;

namespace UnitTests
{
    public class IntegrationTest
    {
        public IntegrationTest()
        {
            Settings.Storage = StorageType.FileSystem;
        }

        [Fact]
        public void Check()
        {
            var mainFolder = new Folder("");
            var size = mainFolder.GetSize();
            Assert.Equal(25, size);
        }
    }
}
