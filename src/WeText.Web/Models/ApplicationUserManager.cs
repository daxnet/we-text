using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeText.Web.Models
{
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private static readonly IWebApiUserStore UserStore =
            new WebApiUserStore("http://localhost:9023/");

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public int AccessFailureCount { get; private set; }

        public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var passwordHash = this.PasswordHasher.HashPassword(password);
            return await UserStore.AuthenticateAsync(user, passwordHash);
        }

        public override async Task<IdentityResult> AccessFailedAsync(string userId)
        {
            var applicationUser = await UserStore.FindByIdAsync(userId);
            if (applicationUser == null)
            {
                throw new InvalidOperationException(string.Format("Account '{0}' does not exist.", userId));
            }
            AccessFailureCount = await UserStore.IncrementAccessFailedCountAsync(applicationUser);
            if (AccessFailureCount >= this.MaxFailedAccessAttemptsBeforeLockout)
            {
                await
                    UserStore.SetLockoutEndDateAsync(applicationUser,
                        DateTimeOffset.UtcNow.Add(this.DefaultAccountLockoutTimeSpan));
                await UserStore.ResetAccessFailedCountAsync(applicationUser);
            }
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            try
            {
                var identityResult = await this.UserValidator.ValidateAsync(user);
                if (!identityResult.Succeeded)
                {
                    return identityResult;
                }
                await this.Store.CreateAsync(user);
                return IdentityResult.Success;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public override bool SupportsQueryableUsers
        {
            get { return false; }
        }

        public override bool SupportsUserClaim
        {
            get { return false; }
        }

        public override bool SupportsUserEmail
        {
            get { return true; }
        }

        public override bool SupportsUserLockout
        {
            get { return false; }
        }

        public override bool SupportsUserLogin
        {
            get { return false; }
        }

        public override bool SupportsUserPassword
        {
            get { return true; }
        }

        public override bool SupportsUserPhoneNumber
        {
            get { return false; }
        }

        public override bool SupportsUserRole
        {
            get { return false; }
        }

        public override bool SupportsUserSecurityStamp
        {
            get { return false; }
        }

        public override bool SupportsUserTwoFactor
        {
            get { return false; }
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager =
                new ApplicationUserManager(UserStore);

            // var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };


            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            manager.PasswordHasher = new ApplicationPasswordHasher();

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}