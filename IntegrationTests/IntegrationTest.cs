using System;
using System.Collections.Generic;
using System.IO;
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
            Storage.Type = StorageType.FileSystem;
            Settings.MainFolder = @"C:\Temp\FileApp\";
        }

        [Fact]
        public void GetSize_SelectMainFolder_CheckSize()
        {
            var mainFolder = new Folder("");
            var size = mainFolder.GetSize();
            Assert.Equal(25, size);
        }


        [Fact]
        public void Rename()
        {
//            var path = @"C:\Temp\FileApp\First\";
//            var o = path + "Second";
//            var n = path + "Second1";

            //new DirectoryInfo(o).MoveTo(n);


            //            var mainFolder = new Folder("");
            //            var list = mainFolder.GetChildren();
            //            var size = mainFolder.GetSize();
            //            Assert.Equal(25, size);
        }

        //TODO: Подумать, как протестировать создание и удаление файлов и директорий
        //        [Fact]
        //        public void CreateDirectory_CheckIfCreated_DeleteDirectory_CheckIfDeleted()
        //        {
        //            var mainFolder = new Folder("");
        //
        //            var dir = new Folder("Test");
        //            mainFolder.Add(dir);
        //
        //            var dirInfo = new DirectoryInfo(Storage.Instance.GetPath(dir.Id) + dir.Name);
        //
        //            Assert.True(dirInfo.Exists);
        //
        //            mainFolder.Remove(dir);
        //
        //            //dir.Delete();
        //
        //            dirInfo = new DirectoryInfo(Storage.Instance.GetPath(dir.Id) + dir.Name);
        //
        //            Assert.True(!dirInfo.Exists);
        //        }
    }
}
