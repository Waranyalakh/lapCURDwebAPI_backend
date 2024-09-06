using System.Security.Cryptography;
using System.Text;

namespace lapCURDwebAPI.Services
{
    public class PasswordService
    {
        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            var computedHash = pbkdf2.GetBytes(20);

            return hash.SequenceEqual(computedHash);
        }
    }
}
