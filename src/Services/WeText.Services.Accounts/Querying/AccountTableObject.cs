using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Services.Accounts.Querying
{
    [ToTable("Accounts")]
    public class AccountTableObject
    {
        [Key(false)]
        public string Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}
