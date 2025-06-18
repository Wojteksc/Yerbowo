namespace Yerbowo.Application.Exceptions.Emails;

public class EmailWasAlreadyConfirmedException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.EmailWasAlreadyConfirmed]) { }