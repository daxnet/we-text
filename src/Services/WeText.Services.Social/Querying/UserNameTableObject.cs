using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Services.Social.Querying
{
    [ToTable("usernames")]
    public class UserNameTableObject
    {
        [Key(false)]
        public string UserId { get; set; }
        public string DisplayName { get; set; }
    }
}
