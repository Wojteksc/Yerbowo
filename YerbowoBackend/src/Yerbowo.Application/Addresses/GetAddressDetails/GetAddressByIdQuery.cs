using MediatR;

namespace Yerbowo.Application.Addresses.GetAddressDetails
{
    public class GetAddressByIdQuery : IRequest<AddressDetailsDto>
    {
        public int Id { get; }

        public GetAddressByIdQuery(int id)
        {
            Id = id;
        }
    }
}
