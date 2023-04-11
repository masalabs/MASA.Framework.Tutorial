using FluentValidation;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Queries;

public class ProductsQueryValidator : AbstractValidator<ProductsQuery>
{
    public ProductsQueryValidator()
    {
        RuleFor(item => item.Page).GreaterThan(0);
        RuleFor(item => item.PageSize).GreaterThan(0);
    }
}