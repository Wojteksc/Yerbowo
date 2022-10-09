namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public class GetAddressByIdQuery : IRequest<AddressDetailsDto>
{
    public int Id { get; }

    public GetAddressByIdQuery(int id)
    {
        Id = id;
    }
}
