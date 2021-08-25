using System.Security.Cryptography;

namespace Yerbowo.Application.Services.Implementations
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool Equals(string newPassword, byte[] currentPassword, byte[] currentSalt)
        {
            using (var hmac = new HMACSHA512(currentSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));

                for (int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if (computedPasswordHash[i] != currentPassword[i])
                        return false;
                }
            }
            return true;
        }
    }
}
