namespace Yerbowo.Application.Exceptions.Newsletters;

public class NewsletterNotFoundException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.NewsletterNotFound])
{ }