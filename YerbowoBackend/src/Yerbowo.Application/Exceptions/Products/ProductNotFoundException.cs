namespace Yerbowo.Application.Exceptions.Products;

public class ProductNotFoundException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.ProductNotFound]) { }