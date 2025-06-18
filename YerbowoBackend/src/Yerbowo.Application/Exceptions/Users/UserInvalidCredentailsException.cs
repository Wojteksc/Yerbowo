namespace Yerbowo.Application.Exceptions.Users;

public class UserInvalidCredentailsException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.UserInvalidCredentails]) { }