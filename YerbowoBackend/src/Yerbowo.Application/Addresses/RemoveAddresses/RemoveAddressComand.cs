using MediatR;

namespace Yerbowo.Application.Addresses.RemoveAddresses
{
    public class RemoveAddressComand : IRequest
    {
        public int Id { get; }

        public RemoveAddressComand(int id)
        {
            Id = id;
        }
    }
}
