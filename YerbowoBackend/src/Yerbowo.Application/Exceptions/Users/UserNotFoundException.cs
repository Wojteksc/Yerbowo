namespace Yerbowo.Application.Exceptions.Users;

public class UserNotFoundException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.UserNotFound]) { }