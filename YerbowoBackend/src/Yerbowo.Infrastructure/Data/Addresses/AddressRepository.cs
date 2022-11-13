namespace Yerbowo.Infrastructure.Data.Addresses;

public class AddressRepository : DbEntityRepository<Address>, IAddressRepository
{
	public AddressRepository(YerbowoContext db) : base(db)
	{
	}

	public async Task<IEnumerable<Address>> GetAddresses(int userId)
	{
		return await _entitiesNotRemoved
			.Where(a => a.UserId == userId)
			.AsNoTracking()
			.ToListAsync();
	}
}