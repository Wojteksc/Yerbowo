namespace Yerbowo.Infrastructure.DAL.Repositories.Users;

public class UserRepository : DbEntityRepository<User>, IUserRepository
{
    public UserRepository(YerbowoContext db) : base(db)
    {
    }

    public async Task<User> GetActiveByEmailAsync(string email)
    {
        return await _entitiesNotRemoved
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _entities
            .SingleOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _entities
            .AnyAsync(x => x.Email == email);
    }
}