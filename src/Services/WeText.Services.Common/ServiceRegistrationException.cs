using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;

namespace WeText.Services.Common
{
    public class ServiceRegistrationException : WeTextException
    {
        public ServiceRegistrationException()
        { }

        public ServiceRegistrationException(string message) : base(message)
        { }

        public ServiceRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ServiceRegistrationException(string format, params object[] args)
            : base(string.Format(format, args))
        { }
    }
}
