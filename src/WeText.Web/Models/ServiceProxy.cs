using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Web.Models
{
    public class ServiceProxy : HttpClient
    {
        public ServiceProxy(string serviceBaseAddress)
            : base()
        {
            this.ServiceBaseAddress = serviceBaseAddress;
            this.BaseAddress = new Uri(serviceBaseAddress.EndsWith("/") ? serviceBaseAddress : serviceBaseAddress + "/");
            this.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string ServiceBaseAddress { get; private set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
