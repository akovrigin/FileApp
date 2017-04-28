using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CoreLibrary;

namespace UnitTests
{
    public class FolderTest
    {
        public class Data : IEnumerable<object[]>
        {
            public Data()
            {
                _mainFolder = new Folder("First")
                    .Add(_secondFolder = new Folder("Second")
                        .Add(_firstFile = new File("test_s1.txt", null) {Size = 10})
                        .Add(new File("test_s2.txt", null) {Size = 5})
                        .Add(new File("test_s3.txt", null) {Size = 1})
                    )
                    .Add(_thirdFolder = new Folder("Third")
                        .Add(_secondFile = new File("test_t1.txt", null) {Size = 2})
                        .Add(new File("test_t2.txt", null) {Size = 4})
                        .Add(_fourthFolder = new Folder("Fourth")
                            .Add(_thirdFile = new File("test_ti1.txt", null) {Size = 3})
                            .Add(_fifthFolder = new Folder("Fifth"))
                        )
                    );
            }

            public IEnumerable<object[]> GetSizeData()
            {
                yield return new object[] { _mainFolder, 25 };
                yield return new object[] { _secondFolder, 16 };
                yield return new object[] { _thirdFolder, 9 };
                yield return new object[] { _fourthFolder, 3 };
                yield return new object[] { _fifthFolder, 0 };
            }

            public IEnumerator<object[]> GetEnumerator()
            {
                return GetSizeData().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public FolderTest()
        {
            Settings.Storage = StorageType.Memory;
        }

        private static IContainer _mainFolder;

        static IContainer _secondFolder;
        static IContainer _thirdFolder;
        static IContainer _fourthFolder;
        static IContainer _fifthFolder;

        static File _firstFile;
        static File _secondFile;
        static File _thirdFile;

        [Theory]
        //[MemberData(nameof(GetSizeData))]
        [ClassData(typeof(Data))]
        public void GetSize_AddElements_CheckSize(IContainer folder, int expectedSize)
        {
            Assert.Equal(folder.GetSize(), expectedSize);
        }

        [Fact]
        public void Copy_CopyFolder_CheckStructure()
        {
            var copiedFolder = ((ICopiable)_thirdFolder).Copy(_mainFolder);

            Assert.Equal("Copy of " + _thirdFolder.Name, copiedFolder.Name);

            var children = ((IContainer)copiedFolder).GetChildren();

            var filesCount = children.Count(e => e is File);
            Assert.Equal(2, filesCount);

            var fourthFolderCopy = (IContainer)children.FirstOrDefault(e => e is Folder && e.Name == "Fourth");

            if (fourthFolderCopy == null)
                Assert.True(false, "No first level nested folder after copy");
            else
            {
                var nestedFolderFilesCount = fourthFolderCopy.GetChildren().Count(e => e is File);

                Assert.Equal(1, nestedFolderFilesCount);

                var fifthFolderCopy = (IContainer)fourthFolderCopy
                    .GetChildren()
                    .FirstOrDefault(e => e is Folder && e.Name == "Fifth");

                if (fifthFolderCopy == null)
                    Assert.True(false, "No second level nested folder after copy");
                else
                {
                    // Id of the copied folder and the original folder the same
                    Assert.NotEqual(_fifthFolder.Id, fifthFolderCopy.Id);

                    // Name of the copied folder and the original folder are not the same
                    Assert.Equal(_fifthFolder.Name, fifthFolderCopy.Name);
                }
            }
        }

        [Fact]
        public void Copy_CopyFile_CheckStructure()
        {
            var copiedFile = _firstFile.Copy(_mainFolder);

            Assert.True(!(copiedFile is IContainer), "Was expected file, but actually it is folder");

            Assert.NotEqual(copiedFile.Id, _firstFile.Id);

            Assert.Equal(10, copiedFile.GetSize());
        }
    }
}
