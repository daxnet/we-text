using Microsoft.Owin.Hosting;
using System;

namespace WeText.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new WeTextService()) service.Start(args);
        }
    }
}
