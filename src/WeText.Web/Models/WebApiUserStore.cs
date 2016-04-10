namespace WeText.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Newtonsoft.Json;
    using WeText.Common;
    /// <summary>
    /// Represents the user store which utilizes the RESTful service to manage the application users.
    /// </summary>
    internal sealed class WebApiUserStore : DisposableObject, IWebApiUserStore
    {
        private readonly string baseAddress;
        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiUserStore"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="serviceBaseAddress">The service base address.</param>
        public WebApiUserStore(string serviceBaseAddress)
        {
            this.baseAddress = serviceBaseAddress;
        }

        public async Task<bool> AuthenticateAsync(ApplicationUser user, string passwordHash)
        {
            try
            {
                using (var proxy = new ServiceProxy(this.baseAddress))
                {
                    var result = await proxy.GetAsync($"api/accounts/name/{user.UserName}");
                    result.EnsureSuccessStatusCode();
                    dynamic accounts = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    dynamic account = accounts[0];
                    return passwordHash == (string)account.Password;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="T:System.Boolean" /> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public async Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return await Task.Factory.StartNew(() => true);
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            await Task.Run(() => user.PasswordHash = passwordHash);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                var result = await proxy.GetAsync(string.Format("api/accounts/email/{0}/", email));
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                try
                {
                    result.EnsureSuccessStatusCode();
                    dynamic account = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    return new ApplicationUser
                    {
                        UserName = (string)account.Name,
                        DisplayName = (string)account.DisplayName,
                        Email = (string)account.Email,
                        Id = (string)account.ID,
                        PasswordHash = (string)account.PasswordHash
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<string> GetEmailAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return await Task.FromResult(true);
        }

        public async Task SetEmailAsync(ApplicationUser user, string email)
        {
            await Task.Run(() => { });
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            await Task.Run(() => { });
        }

        #region IUserStore<ApplicationUser,string> Members

        /// <summary>
        /// Creates the application user.
        /// </summary>
        /// <param name="user">The user that is going to be created.</param>
        /// <returns>A task which is responsible for creating the application user.</returns>
        public async Task CreateAsync(ApplicationUser user)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                var result = await proxy.PostAsJsonAsync("api/accounts/create", new
                {
                    Name = user.UserName,
                    user.Email,
                    Password = user.PasswordHash,
                    DisplayName = user.UserName
                });
                result.EnsureSuccessStatusCode();
                var id = await result.Content.ReadAsStringAsync();
                user.Id = JsonConvert.DeserializeObject<string>(id);
            }
        }

        /// <summary>
        /// Deletes the specified application user.
        /// </summary>
        /// <param name="user">The user to be deleted.</param>
        /// <returns>A task that is responsible for deleting the application user.</returns>
        public async Task DeleteAsync(ApplicationUser user)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
                await proxy.DeleteAsync(string.Format("api/accounts/delete/{0}", user.Id));
        }

        /// <summary>
        /// Finds the application user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A task that is responsible for finding the application user by its identifier and returns
        /// the found user instance.</returns>
        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                var result = await proxy.GetAsync(string.Format("api/accounts/id/{0}", userId));
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                try
                {
                    result.EnsureSuccessStatusCode();
                    dynamic accounts = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    dynamic account = accounts[0];
                    return new ApplicationUser
                    {
                        UserName = (string)account.Name,
                        DisplayName = (string)account.DisplayName,
                        Email = (string)account.Email,
                        Id = (string)account.Id,
                        PasswordHash = (string)account.Password
                    };
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Finds the application user by name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>A task that is responsible for finding the application user by its name and returns
        /// the found user instance.</returns>
        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                var result = await proxy.GetAsync(string.Format("api/accounts/name/{0}", userName));
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                try
                {
                    result.EnsureSuccessStatusCode();
                    dynamic accounts = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                    dynamic account = accounts[0];
                    return new ApplicationUser
                    {
                        UserName = (string)account.Name,
                        DisplayName = (string)account.DisplayName,
                        Email = (string)account.Email,
                        Id = (string)account.Id,
                        PasswordHash = (string)account.Password
                    };
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Updates the application user.
        /// </summary>
        /// <param name="user">The user to be updated.</param>
        /// <returns>A task that is responsible for updating the user.</returns>
        public async Task UpdateAsync(ApplicationUser user)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                await proxy.PostAsJsonAsync(string.Format("api/accounts/update/{0}", user.Id), new
                {
                    user.DisplayName,
                    user.Email
                });
            }
        }

        #endregion

        #region IUserLockoutStore<ApplicationUser,string> Members

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            //using (var proxy = new ServiceProxy(this.baseAddress))
            //{
            //    dynamic account =
            //    JsonConvert.DeserializeObject(await proxy.GetStringAsync(string.Format("api/accounts/id/{0}", user.Id)));
            //    return Convert.ToInt32(account.PasswordFailures);
            //}
            return await Task.FromResult(0);
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return await Task.FromResult(false);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                dynamic account =
                JsonConvert.DeserializeObject(await proxy.GetStringAsync(string.Format("api/accounts/id/{0}", user.Id)));
                var lastLockedDate = (DateTime?)account.DateLockedEnd;
                var lockoutEndDate = lastLockedDate.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(lastLockedDate.Value, DateTimeKind.Utc))
                    : DateTimeOffset.MinValue;
                return lockoutEndDate;
            }
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            //using (var proxy = new ServiceProxy(this.baseAddress))
            //{
            //    var result = await proxy.PostAsJsonAsync(string.Format("api/accounts/passwd/failures/increment/{0}", user.Id), string.Empty);
            //    return await result.Content.ReadAsAsync<int>();
            //}
            return await Task.FromResult(0);
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            //using (var proxy = new ServiceProxy(this.baseAddress))
            //{
            //    await proxy.PostAsJsonAsync(string.Format("api/accounts/passwd/failures/reset/{0}", user.Id), string.Empty);
            //}
            await Task.CompletedTask;
        }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            await Task.FromResult(0);
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            using (var proxy = new ServiceProxy(this.baseAddress))
            {
                var lockEndDate = lockoutEnd.UtcDateTime;
                await proxy.PutAsJsonAsync(string.Format("api/accounts/update/{0}", user.Id), new
                {
                    LockoutEndDate = lockEndDate
                });
            }
        }

        #endregion

        #region IUserTwoFactorStore<ApplicationUser,string> Members

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return await Task.FromResult(false);
        }

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            await Task.FromResult(0);
        }

        #endregion

        #region IUserPhoneNumberStore<ApplicationUser,string> Members

        public async Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return await Task.FromResult(string.Empty);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return await Task.FromResult(true);
        }

        public async Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            await Task.FromResult(0);
        }

        public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            await Task.FromResult(0);
        }

        #endregion

        #region IUserLoginStore<ApplicationUser,string> Members

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            await Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return await Task.FromResult(new ApplicationUser());
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return await Task.FromResult(new List<UserLoginInfo>());
        }

        public async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            await Task.FromResult(0);
        }

        #endregion
    }
}