using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Services
{
    public interface IService : IDisposable
    {
        void Start(object[] args);
    }
}
