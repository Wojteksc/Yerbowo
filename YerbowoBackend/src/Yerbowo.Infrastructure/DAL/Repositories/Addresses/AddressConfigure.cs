namespace Yerbowo.Infrastructure.DAL.Repositories.Addresses;

public class AddressConfigure : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasOne(a => a.User);
    }
}