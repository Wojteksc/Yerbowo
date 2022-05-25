using Yerbowo.Domain.Addresses;

namespace Yerbowo.Infrastructure.Data.Addresses;

public class AddressConfigure : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasOne(a => a.User);
    }
}