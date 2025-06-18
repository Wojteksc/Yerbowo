namespace Yerbowo.Application.Exceptions.Products;

public class ProductNameIsAlreadyExistsException(IStringLocalizer<SharedResource> localizer)
    : CustomException(localizer[Localizations.ProductNameIsAlreadyExists]) { }