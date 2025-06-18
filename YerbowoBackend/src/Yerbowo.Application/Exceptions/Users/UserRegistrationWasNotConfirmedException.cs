namespace Yerbowo.Application.Exceptions.Users;

public class UserRegistrationWasNotConfirmedException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.UserRegistrationWasNotConfirmed]) { }