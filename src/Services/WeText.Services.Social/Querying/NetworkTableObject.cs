using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Services.Social.Querying
{
    [ToTable("Networks")]
    public class NetworkTableObject
    {
        [Key(false)]
        public string InvitationId { get; set; }

        public string OriginatorId { get; set; }

        public string TargetId { get; set; }

        public string OriginatorName { get; set; }

        public string TargetUserName { get; set; }

        public DateTime InvitationStartDate { get; set; }

        public DateTime InvitationEndDate { get; set; }

        public InvitationEndReason InvitationEndReason { get; set; }
    }
}
