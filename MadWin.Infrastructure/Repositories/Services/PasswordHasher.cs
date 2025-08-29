
using System.Text;
using System.Security.Cryptography;
using MadWin.Application.Services;

namespace MadWin.Infrastructure.Repositories.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword (string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return string.Concat(hash.Select(b => b.ToString("x2")));
            }
        }
    }
}
