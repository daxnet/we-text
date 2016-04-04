using Microsoft.Owin.Hosting;
using System;

namespace WeText.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://+:9023/";
            using (WebApp.Start<Startup>(url: url))
            {
                Console.WriteLine("Service started.");
                Console.ReadLine();
            }
        }
    }
}
