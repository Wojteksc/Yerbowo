namespace Yerbowo.Application.Exceptions;

public class InvalidTokenException(IStringLocalizer<SharedResource> localizer) 
    : CustomException(localizer[Localizations.InvalidToken]) { }