namespace Yerbowo.Application.Exceptions.Emails;

public class EmailIsAlreadySubscribedToNewsletterException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.EmailIsAlreadySubscribed]) { }