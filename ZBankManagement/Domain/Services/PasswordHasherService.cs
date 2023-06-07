using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZBank.ZBankManagement.Services.Contracts;

namespace ZBank.ZBankManagement.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly int _saltBytes = 100;

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10101))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(70));
            }
        }

        public string GenerateSalt()
        {
            var saltBytes = new byte[_saltBytes];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }
    }
}
