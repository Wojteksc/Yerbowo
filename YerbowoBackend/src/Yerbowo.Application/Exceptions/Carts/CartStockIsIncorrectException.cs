namespace Yerbowo.Application.Exceptions.Carts;

public class CartStockIsIncorrectException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.CartStockIsIncorrect]) { }