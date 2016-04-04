using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Events
{
    /// <summary>
    /// Represents that the decorated method is an event handler within an aggregate root.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class InlineEventHandlerAttribute : Attribute
    {

    }
}
