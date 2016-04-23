using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WeText.Web.Models
{
    public static class ConfigReader
    {
        private static string serviceUrl;

        static ConfigReader()
        {
            serviceUrl = ConfigurationManager.AppSettings["wetext:ServiceUrl"];
        }

        public static string ServiceUrl => serviceUrl;
    }
}