using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Commands
{
    public class CreateUserCommand : Command
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}
