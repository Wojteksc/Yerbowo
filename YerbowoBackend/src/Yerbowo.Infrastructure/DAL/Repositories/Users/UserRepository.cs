using Yerbowo.Infrastructure.DAL.Repositories;

namespace Yerbowo.Infrastructure.DAL.Repositories.Users;

public class UserRepository : DbEntityRepository<User>, IUserRepository
{
    public UserRepository(YerbowoContext db) : base(db)
    {
    }

    public async Task<User> GetAsync(string email)
    {
        return await _entities.SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _entities.AnyAsync(x => x.Email == email);
    }
}