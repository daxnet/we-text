using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Config
{
    public class WeTextConfigurationException : WeTextException
    {
        public WeTextConfigurationException() { }

        public WeTextConfigurationException(string message) : base(message)
        { }

        public WeTextConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public WeTextConfigurationException(string format, params object[] args)
            : base(string.Format(format, args))
        { }
    }
}
