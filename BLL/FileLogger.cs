using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class FileLogger : ILogger
    {

        private readonly string path = @"D:\FileManager.log";
        public void Log(string message)
        {
            File.AppendAllText(path, string.Format($"{message}{Environment.NewLine}"));
        }
    }
}