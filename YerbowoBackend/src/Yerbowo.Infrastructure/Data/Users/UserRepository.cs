using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Users
{
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
            if (await _entities.AnyAsync(x => x.Email == email))
                return true;

            return false;
        }
    }
}
