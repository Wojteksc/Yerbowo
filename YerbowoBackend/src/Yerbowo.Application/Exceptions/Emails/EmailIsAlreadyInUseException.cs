namespace Yerbowo.Application.Exceptions.Emails;

public class EmailIsAlreadyInUseException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.EmailIsAlreadyInUse]) { }