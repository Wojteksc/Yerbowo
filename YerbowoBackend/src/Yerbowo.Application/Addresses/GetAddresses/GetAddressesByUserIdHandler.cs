using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Application.Addresses.GetAddresses
{
    internal class GetAddressesByUserIdHandler : IRequestHandler<GetAddressesByUserIdQuery, IEnumerable<AddressDto>>
	{
        private readonly IMapper _mapper;
        private readonly IAddressRepository _addressRepository;

        public GetAddressesByUserIdHandler(IMapper mapper,
            IAddressRepository addressRepository)
        {
            _mapper = mapper;
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<AddressDto>> Handle(GetAddressesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _addressRepository.GetAddresses(request.UserId);

            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }
    }
}
