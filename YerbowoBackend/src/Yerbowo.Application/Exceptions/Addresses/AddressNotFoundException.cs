namespace Yerbowo.Application.Exceptions.Addresses;

public class AddressNotFoundException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.AddressNotFound]) { }