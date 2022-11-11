namespace Yerbowo.Infrastructure.Data.Addresses
{
	public interface IAddressRepository : IEntityRepository<Address>
	{
		Task<IEnumerable<Address>> GetAddresses(int userId);
	}
}