namespace Yerbowo.Application.Exceptions.Emails;

public class EmailSendingFailedException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.EmailSendingFailedException]) { }