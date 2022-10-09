namespace Yerbowo.Application.Functions.Addresses.Command.RemoveAddresses;

public class RemoveAddressComand : IRequest
{
    public int Id { get; }

    public RemoveAddressComand(int id)
    {
        Id = id;
    }
}
