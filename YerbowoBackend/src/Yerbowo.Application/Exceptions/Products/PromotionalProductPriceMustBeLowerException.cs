namespace Yerbowo.Application.Exceptions.Products;

public class PromotionalProductPriceMustBeLowerException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.PromotionalProductPriceMustBeLowerThanCurrent]) { }