namespace Yerbowo.Application.Abstractions.Repositories;

public interface IUserRepository : IDbEntityRepository<User>
{
    Task<User> GetAsync(string email);
    Task<bool> ExistsAsync(string email);
}