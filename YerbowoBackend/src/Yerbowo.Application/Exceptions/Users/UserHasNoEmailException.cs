namespace Yerbowo.Application.Exceptions.Users;

public class UserHasNoEmailException(IStringLocalizer<SharedResource> localizer, string provider)
    : CustomException(string.Format(localizer[Localizations.UserHasNoEmail], provider)) { }