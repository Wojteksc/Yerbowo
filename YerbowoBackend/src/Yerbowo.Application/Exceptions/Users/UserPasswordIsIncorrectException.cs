namespace Yerbowo.Application.Exceptions.Users;

public class UserPasswordIsIncorrectException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.UserPasswordIsIncorrect]) { }