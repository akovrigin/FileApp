using System.IO;
using System.Linq;
using CoreLibrary;
using Xunit;

namespace IntegrationTests
{
    public class IntegrationTest
    {
        public void Init()
        {
            Storage.Type = StorageType.FileSystem;
            Settings.MainFolder = @"C:\Temp\FileApp\";
            Settings.RelativePath = "";
        }

        [Fact]
        public void GetSize_SelectMainFolder_CheckSize()
        {
            Init();

            var mainFolder = new Folder("");
            var size = mainFolder.GetSize();
            Assert.Equal(25, size);
        }

        [Fact]
        public void GetFolder_Rename_CheckIfRenamed()
        {
            Init();

            Settings.RelativePath = "First";
            var o = Settings.FullPath + "Second";
            var n = Settings.FullPath + "Second1";

            var dir1 = new DirectoryInfo(o);
            var dir2 = new DirectoryInfo(n);

            Assert.True(dir1.Exists, $"Folder doesn't exists: {dir1.FullName}");
            Assert.False(dir2.Exists, $"Folder does exists: {dir2.FullName}");

            var folder = new Folder("Second");
            folder.Rename("Second1");

            var dir3 = new DirectoryInfo(o);
            var dir4 = new DirectoryInfo(n);

            Assert.True(dir4.Exists, $"Folder doesn't exists: {dir4.FullName}");
            Assert.False(dir3.Exists, $"Folder does exists: {dir3.FullName}");

            folder.Rename("Second");
        }

        [Fact]
        public void CreateDirectory_CheckIfCreated_DeleteDirectory_CheckIfDeleted()
        {
            Init();

            CreateDirectory_CheckIfCreated();
            DeleteDirectory_CheckIfDeleted();
        }

        public void CreateDirectory_CheckIfCreated()
        {
            var mainFolder = new Folder("First");

            var dir = new Folder("Test");
            mainFolder.Add(dir);

            Settings.RelativePath = "First";

            var dirInfo = new DirectoryInfo(Settings.FullPath + dir.Name);

            Assert.True(dirInfo.Exists);
        }

        public void DeleteDirectory_CheckIfDeleted()
        {
            Settings.RelativePath = "";

            var mainFolder = new Folder("First");

            var folder = mainFolder.GetChildren().First(c => c.Name == "Test");

            folder.Delete();

            var dirInfo = new DirectoryInfo(Settings.FullPath + folder.Name);

            Assert.False(dirInfo.Exists);
        }

        [Fact]
        public void CopyDirectory_DirectoriesAndFilesRecursivelyCreated()
        {
            Init();

            var parent = new Folder("Third");

            var folder = parent.GetChildren().First(c => c.Name == "Fourth");

            Settings.RelativePath = "Third";

            ((ICopiable)folder).Copy(parent);

            var path = @"C:\Temp\FileApp\Third\Copy of Fourth\";
            var di = new DirectoryInfo(path);
            Assert.True(di.Exists);

            di = new DirectoryInfo(path + @"Fifth\");
            Assert.True(di.Exists);

            path = @"C:\Temp\FileApp\Third\Copy of Fourth\";
            var fi = new FileInfo(path + "file_f1.txt");
            Assert.True(fi.Exists);

            Directory.Delete(path, true);
        }

        [Fact]
        public void CopyFile_FileCopied()
        {
            Init();

            var parent = new Folder("Third");

            var folder = parent.GetChildren().First(c => c.Name == "file_t1.txt");

            Settings.RelativePath = "Third";

            ((ICopiable)folder).Copy(parent);

            var path = @"C:\Temp\FileApp\Third\Copy of file_t1.txt";

            Assert.True(new FileInfo(path).Exists);

            System.IO.File.Delete(path);
        }
    }
}
