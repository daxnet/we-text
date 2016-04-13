using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Domain.Commands
{
    public class ChangeTextCommand : Command
    {
        public Guid TextId;

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
