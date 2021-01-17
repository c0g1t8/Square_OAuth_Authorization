using System;
using Microsoft.AspNetCore.Hosting;

namespace Authorize
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseUrls("http://*:5000")
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
