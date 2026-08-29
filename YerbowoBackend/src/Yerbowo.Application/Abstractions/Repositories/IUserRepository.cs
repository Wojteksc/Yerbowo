namespace Yerbowo.Application.Abstractions.Repositories;

public interface IUserRepository : IDbEntityRepository<User>
{
    Task<User> GetActiveByEmailAsync(string email);
    Task<User> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}