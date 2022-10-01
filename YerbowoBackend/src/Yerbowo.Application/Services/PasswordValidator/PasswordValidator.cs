namespace Yerbowo.Application.Services.PasswordValidator;

public class PasswordValidator : IPasswordValidator
{
    public bool Equals(string newPassword, byte[] currentPassword, byte[] currentSalt)
    {
        using (var hmac = new HMACSHA512(currentSalt))
        {
            var computedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            return computedPasswordHash.SequenceEqual(currentPassword);
        }
    }
}