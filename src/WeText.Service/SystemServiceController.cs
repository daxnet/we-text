using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WeText.Service
{
    [RoutePrefix("api")]
    public sealed class SystemServiceController : ApiController
    {
        [HttpGet]
        [Route("system/info")]
        public IHttpActionResult GetSystemInfo()
        {
            return Ok(new
            {
                Environment.MachineName,
                Environment.Is64BitOperatingSystem,
                Environment.Is64BitProcess,
                Environment.OSVersion.VersionString
            });
        }
    }
}
