namespace Yerbowo.Application.Services.PasswordValidator;

public interface IPasswordValidator
{
    bool Equals(string newPassword, byte[] currentPassword, byte[] currentPasswordSalt);
}
