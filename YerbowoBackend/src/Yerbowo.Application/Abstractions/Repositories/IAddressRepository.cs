namespace Yerbowo.Application.Abstractions.Repositories;

public interface IAddressRepository : IDbEntityRepository<Address>
{
	Task<IEnumerable<Address>> GetAddresses(int userId);
}