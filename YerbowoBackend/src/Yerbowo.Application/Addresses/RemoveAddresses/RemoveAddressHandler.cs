using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Application.Addresses.RemoveAddresses
{
    public class RemoveAddressHandler : IRequestHandler<RemoveAddressComand>
    {
        private readonly IAddressRepository _addressRepository;

        public RemoveAddressHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Unit> Handle(RemoveAddressComand request, CancellationToken cancellationToken)
        {
            var address = await _addressRepository.GetAsync(request.Id);

            if (address == null || address.IsRemoved)
            {
                throw new Exception("Adres nie istnieje");
            }

            await _addressRepository.RemoveAsync(address);

            return Unit.Value;
        }
    }
}
