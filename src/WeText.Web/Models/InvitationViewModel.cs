using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Web.Models
{
    public class InvitationViewModel
    {
        public Guid InvitationId { get; set; }

        public string UserDisplayName { get; set; }

        public DateTime SentDate { get; set; }

        public string Status { get; set; }

        public DateTime? CompleteDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
