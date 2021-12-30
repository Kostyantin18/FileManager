using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    public class FileManager 
    {
        public FileManager()
        {

        }
        private readonly ILogger _logger;
        public FileManager(ILogger logger)
        {
            _logger = logger;
        }



        public string dirname { get; set; } = @"D:\";
        public bool stop { get; set; } = false;

        

        public void CommandList()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Here are all the commands: ");
            Console.WriteLine("'open' to open directory/file");
            Console.WriteLine("'open df' to specify which directory/file you want to open");
            Console.WriteLine("'create' to create folder/file");
            Console.WriteLine("'change directory' or 'cd' to change directory");
            Console.WriteLine("'get current directory' or 'gcd' to get current directory");
            Console.WriteLine("'back' to go to the parent directory");
            Console.WriteLine("'clear' to clear the console");
            Console.WriteLine("'info' to get some information about all files in this folder");
            Console.WriteLine("'stop' to finish");
            Console.ForegroundColor = ConsoleColor.White;

            string operation = "";
            operation = Console.ReadLine();
            Console.WriteLine();
           
            switch (operation)
            {
                case "stop":
                    this.Stop();
                    break;
                case "back":
                    this.Back();
                    break;
                case "open":
                    this.Open();
                    break;
                case "open df":
                    this.OpenDF();
                    break;
                case "info":
                    this.GetInfoFiles();
                    break;
                case "create":
                    this.Create();
                    break;
                case "rename":
                    this.Rename();
                    break;
                case "delete":
                    this.Delete();
                    break;
                case "change directory":
                case "cd":
                    Console.WriteLine();
                    this.ChangeDirectory();
                    break;
                case "clear":
                    this.Clear();
                    break;
                case "get current directory":
                case "gcd":
                    this.GetCurrentDirectory();
                    break;
                default:
                    Console.WriteLine("No such operation");
                    break;
            }
        }

        private void Open()
        {
            if (File.Exists(dirname))
            {
                using (FileStream fs = File.OpenRead(dirname))
                {
                    byte[] arr = new byte[fs.Length];
                    fs.Read(arr, 0, arr.Length);
                    string text = System.Text.Encoding.Default.GetString(arr);
                    Console.WriteLine(text);
                }
                    
                _logger.Log($"{dirname} opened");
                return;
            }
            this.GetDirectories();
            this.GetFiles();
            _logger.Log($"{dirname} opened");
        }

        private void OpenDF()
        {
            
            this.GetDirectories();
            this.GetFiles();
            Console.WriteLine();

            
            Console.WriteLine("Enter the name of the folder or file: ");
            string name = Console.ReadLine();

            char backslash = dirname[dirname.Length - 1];
            if (backslash != Convert.ToChar(@"\"))
            {
                dirname += @"\";
            }

            string temp = dirname + name;

            Console.WriteLine();
            if (Directory.Exists(temp))
            {

                dirname = temp;
                dirname += @"\";
                this.GetDirectories();
                this.GetFiles();
                

            } else if (File.Exists(temp))
            {
                dirname = temp;
                string text = File.ReadAllText(dirname);
                if (text.Length < 200) Console.WriteLine(text);
                else Console.WriteLine("File's length is more than 200 symbols");
                
            }
            else
            {
                Console.WriteLine("invalid name");
                return;
            }
            _logger.Log($"{dirname} opened");
        }

        private void GetInfoFiles()
        {
            string[] files = Directory.GetFiles(dirname);
            if (files.Length == 0) return;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Information about files: ");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (string file in files)
            {
                Console.WriteLine(file);

                FileInfo fileInfo = new FileInfo(file);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\tsize: {fileInfo.Length} bytes");
                Console.WriteLine($"\tcreated: {fileInfo.CreationTime}");
                Console.WriteLine($"\tlast updated: {fileInfo.LastWriteTime}");
                
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private void Clear()
        {
            Console.Clear();
        }

        public void ChangeDirectory()
        {
            Console.WriteLine("Enter the name of the directory: (C/D)");

            string name = Console.ReadLine() + @":\";
            if (!Directory.Exists(name))
            {
                Console.WriteLine("Incorrect input");
                Console.WriteLine("Set to 'D:\\'");
                Console.WriteLine("type 'cd' to change");
                return;
            }

            _logger.Log($"Directory changed from {dirname} to {name}");
            dirname = name;


        }

        private void GetDirectories()
        {
            if (!Directory.Exists(dirname)) return;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Folders: ");
            Console.ForegroundColor = ConsoleColor.White;
            string[] dirs = Directory.GetDirectories(dirname);
            if (dirs.Length == 0)
            {
                Console.WriteLine("There are no other folders in this one.");
            }
            foreach (string dir in dirs) Console.WriteLine(dir);
        }

        private void GetFiles()
        {
            if (!Directory.Exists(dirname) && !File.Exists(dirname)) return;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Files: ");
            Console.ForegroundColor = ConsoleColor.White;

            if (Directory.GetFiles(dirname).Length != 0)
            {
                string[] files = Directory.GetFiles(dirname);
                foreach (string file in files) Console.WriteLine(file);
            }
            else
            {
                Console.WriteLine("There are no files in this one.");
            }

        }

        private void GetCurrentDirectory()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(dirname);
            Console.ForegroundColor = ConsoleColor.White;
            _logger.Log($"Current directory: {dirname}");
        }

        public void Create()
        {
            Console.WriteLine("Create a folder or a file?");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "folder":
                    this.CreateFolder();
                    break;
                case "file":
                    this.CreateFile();
                    break;
                default:
                    TryAgain(Create);
                    break;
            }
        }

        public void CreateFolder()
        {
            Console.WriteLine("Enter the name of the folder: ");
            string name = Console.ReadLine();
           
            char backslash = dirname[dirname.Length - 1];
            if (backslash != Convert.ToChar(@"\"))
            {
                dirname += @"\";
            }

            string temp = dirname + name;
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
                dirname = temp;
                dirname += @"\";
                _logger.Log($"Directory {temp} was created");
            }
            else if (Directory.Exists(temp))
            {
                Console.WriteLine("Directory with this name already exists.");
                return;
            }
            else
            {
                TryAgain(CreateFolder);
            }
        }

        public void CreateFile()
        {
            Console.WriteLine("Enter the name of the file (with its extension): ");
            string name = Console.ReadLine();
            //string name = "test.txt";
            char backslash = dirname[dirname.Length - 1];
            if (backslash != Convert.ToChar(@"\"))
            {
                dirname += @"\";
            }

            string temp = dirname + name;
            if (!File.Exists(temp))
            {
                try
                {
                    File.Create(temp);
                }catch(Exception)
                {
                    
                    TryAgain(CreateFile);
                }
                dirname = temp;
                dirname += @"\";
              //  _logger.Log($"File {temp} was created");
                
            }else if (File.Exists(temp))
            {
                Console.WriteLine("File with this name already exists.");
            }
            else
            {
                TryAgain(CreateFile);
            } 
        }
        

        private void TryAgain(Action action)
        {
            Console.WriteLine("Failed. Try again? (y/n)");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y":
                    action.Invoke();
                    break;
                default:
                    break;
            }
        }

        

        public void Delete()
        {
            Console.WriteLine("Which folder/file to delete?");
            string name = Console.ReadLine();
            string temp = dirname + name ;
            if (Directory.Exists(temp))
            {
                if (Directory.GetFiles(temp).Length != 0 || Directory.GetDirectories(temp).Length != 0)
                {
                    //temp += @"\";
                    Console.WriteLine("This directory is not empty. Delete it anyway?(y/n)");
                    if (Console.ReadLine() == "y")
                    {
                        Directory.Delete(temp, true);
                        Console.ForegroundColor = ConsoleColor.Red;
                        _logger.Log($"Directory {temp} was deleted");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }   
                }
                else {
                    Directory.Delete(temp);
                    return;
                }
            } else 
            {
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                    Console.ForegroundColor = ConsoleColor.Red;
                    _logger.Log($"File {temp} was deleted");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }
            this.TryAgain(Delete);
        }

        private void Rename()
        {
            if (Directory.GetParent(dirname)== null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You cannot rename this directory");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            FileAttributes attributes = File.GetAttributes(dirname);

            Console.WriteLine("Enter new name: (type '*cancel' to cancel)");
            Console.WriteLine("When renaming make sure there isn't '\\' at the end of your directory");
            string name = Console.ReadLine();
            if (name == "*cancel") return;

            char backslash = dirname[dirname.Length - 1];
            if (backslash == Convert.ToChar(@"\"))
            {
                Console.WriteLine("type 'back' first, then 'rename'");
                return;
            }


            string oldFile = dirname;
            string oldDirectory = dirname;
            dirname = Directory.GetParent(dirname).FullName;
            string temp = dirname + @"\"  + name;
            if (attributes == FileAttributes.Directory)  temp = dirname + name;
            

            if (attributes == FileAttributes.Directory && !Directory.Exists(temp))
            {
                
                Directory.Move(oldDirectory, temp);
                dirname = temp + @"\";
                _logger.Log($"Directory {oldDirectory} was renamed to {temp}");


            }
            else if (!File.Exists(temp))
            {
               
                File.Move(oldFile, temp);
                dirname = temp;
                _logger.Log($"File {oldFile} was renamed to {temp}");
            }
            else
            {
                dirname += @"\";
                this.TryAgain(Rename);
            }
        }
        
        public void Back()
        {
            if (Directory.GetParent(dirname) == null)
            {
                return;
            }
            dirname = Directory.GetParent(dirname).FullName;
            /*char backslash = dirname[dirname.Length - 1];
            if (backslash != Convert.ToChar(@"\"))
            {
                dirname += @"\";
            }*/
            this.GetCurrentDirectory();
        }

        private void Stop()
        {
            this.stop = true;
        }
    } 
}
