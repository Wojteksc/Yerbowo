namespace Yerbowo.Application.Services
{
    public interface IPasswordValidator
    {
        bool Equals(string newPassword, byte[] currentPassword, byte[] currentPasswordSalt);
    }
}
