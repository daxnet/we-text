using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeText.Web.Models
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public interface IWebApiUserStore : IUserStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser, string>, IUserPasswordStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserTwoFactorStore<ApplicationUser, string>,
        IUserPhoneNumberStore<ApplicationUser>,
        IUserLoginStore<ApplicationUser>
    {
        Task<bool> AuthenticateAsync(ApplicationUser user, string passwordHash);
    }
}