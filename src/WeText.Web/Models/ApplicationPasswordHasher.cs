namespace WeText.Web.Models
{
    using Microsoft.AspNet.Identity;

    public class ApplicationPasswordHasher : IPasswordHasher
    {
        #region IPasswordHasher Members

        public string HashPassword(string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword == HashPassword(providedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }

        #endregion
    }
}
