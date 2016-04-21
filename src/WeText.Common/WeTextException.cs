using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common
{
    public class WeTextException : Exception
    {
        public WeTextException() { }

        public WeTextException(string message) : base(message)
        { }

        public WeTextException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public WeTextException(string format, params object[] args)
            : base(string.Format(format, args))
        { }
    }
}
