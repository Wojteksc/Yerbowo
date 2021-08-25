using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Addresses
{
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
}
