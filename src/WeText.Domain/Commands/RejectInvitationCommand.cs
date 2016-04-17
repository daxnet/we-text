using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Domain.Commands
{
    public class RejectInvitationCommand : Command
    {
        public Guid UserId { get; set; }

        public Guid InvitationId { get; set; }
    }
}
