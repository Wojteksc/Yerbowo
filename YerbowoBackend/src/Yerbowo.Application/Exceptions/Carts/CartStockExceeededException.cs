namespace Yerbowo.Application.Exceptions.Carts;

public class CartStockExceeededException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.CartStockIsExceeded]) { }