using System.Collections.Generic;
using System.Threading.Tasks;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Addresses
{
	public interface IAddressRepository : IEntityRepository<Address>
	{
		Task<IEnumerable<Address>> GetAddresses(int userId);
	}
}
