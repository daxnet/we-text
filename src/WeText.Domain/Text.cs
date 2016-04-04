using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;

namespace WeText.Domain
{
    public class Text : AggregateRoot<int>
    {
        public string Title { get; private set; }

        public string Content { get; private set; }

        public DateTime DateCreated { get; private set; }

        public DateTime? DateModified { get; private set; }

        public User Owner { get; private set; }
    }
}
