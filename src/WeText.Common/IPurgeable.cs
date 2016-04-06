using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common
{
    /// <summary>
    /// Represents that the implemented classes are the objects
    /// that can be purged.
    /// </summary>
    internal interface IPurgeable
    {
        /// <summary>
        /// Performs the purge operation.
        /// </summary>
        void Purge();
    }
}
