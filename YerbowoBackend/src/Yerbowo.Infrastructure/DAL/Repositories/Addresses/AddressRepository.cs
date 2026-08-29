namespace Yerbowo.Infrastructure.DAL.Repositories.Addresses;

public class AddressRepository : DbEntityRepository<Address>, IAddressRepository
{
    public AddressRepository(YerbowoContext db) : base(db)
    {
    }

    public async Task<IEnumerable<Address>> GetAddresses(Guid userId)
    {
        return await _entitiesNotRemoved
            .Where(a => a.UserId == userId)
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ThenByDescending(a => a.Id)
            .ToListAsync();
    }
}