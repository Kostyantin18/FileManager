using BLL;
using Moq;
using System;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class FileManagerTests
    {
        [Fact]
        public void CreateFolder_CreateFolder_FolderCreated()
        {
            var manager = new FileManager();
            var name = "test";
            var oldDirname = manager.dirname;
            var stringReader = new StringReader(name);
            Console.SetIn(stringReader);

            manager.CreateFolder();

            
            Assert.True(Directory.Exists(oldDirname + @"\" + name));
        }

        [Fact]
        public void CreateFile_CreateFile_FileCreated()
        {
            var manager = new FileManager();
            var name = "test.txt";
            var oldDirname = manager.dirname;
            var stringReader = new StringReader(name);
            Console.SetIn(stringReader);

            manager.CreateFile();


            Assert.True(File.Exists(oldDirname + @"\" + name));
        }

        [Fact]
        public void Delete_DeleteFolderOrFile_Deleted()
        {
            var manager = new FileManager();
            var name = "test2";
            var stringReader = new StringReader(name);
            Console.SetIn(stringReader);

            manager.Delete();

            Assert.False(Directory.Exists(manager.dirname + @"\" + name) || File.Exists(manager.dirname + @"\" + name));
        }

        [Fact]
        public void ChangeDirectory_DirectoryDoesntExist_DirectoryNotChanged()
        {
            var manager = new FileManager();
            var name = "X";
            var oldDirname = manager.dirname;
            var stringReader = new StringReader(name);
            Console.SetIn(stringReader);

            manager.ChangeDirectory();

            Assert.Equal(oldDirname, manager.dirname);
        }

        
    }
}
