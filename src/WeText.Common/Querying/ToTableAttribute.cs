using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Querying
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ToTableAttribute : Attribute
    {
        public string Name { get; }

        public ToTableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
