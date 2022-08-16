using ConsoleAppTest.DataAccess;
using ConsoleAppTest.Models;
using ConsoleAppTest.Services;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppTest
{
    static class Program
    {
        public static IConfiguration Configuration;

        [STAThread]
        static void Main()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();

            Console.WriteLine();
        }
    }
}