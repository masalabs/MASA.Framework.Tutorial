using FluentValidation;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Queries;

public class ProductQueryValidator : AbstractValidator<ProductQuery>
{
    public ProductQueryValidator()
    {
        RuleFor(item => item.ProductId).GreaterThan(0).WithMessage("Please enter the ProductId");
    }
}