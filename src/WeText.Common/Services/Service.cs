using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Services
{
    public abstract class Service : IService
    {

        ~Service()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void Start(object[] args);

        protected virtual void Dispose(bool disposing) { }
    }
}
