using BLL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScoped<ILogger, ConsoleLogger>();
            services.AddScoped<BLL.FileManager>();
            var serviceProvider = services.BuildServiceProvider();


            BLL.FileManager FileManager = (BLL.FileManager)serviceProvider.GetService(typeof(BLL.FileManager));

            FileManager.ChangeDirectory();
            while (!FileManager.stop)
            { 
                 FileManager.CommandList();
            }
        }

    }
}
