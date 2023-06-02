namespace Yerbowo.Application.Functions.Addresses.Command.RemoveAddresses;

public class RemoveAddressCommand : IRequest
{
    public int Id { get; }

    public RemoveAddressCommand(int id)
    {
        Id = id;
    }
}
