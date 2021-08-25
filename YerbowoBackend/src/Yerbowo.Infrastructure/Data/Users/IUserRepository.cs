using System.Threading.Tasks;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Users
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<User> GetAsync(string email);
        Task<bool> ExistsAsync(string email);
    }
}
