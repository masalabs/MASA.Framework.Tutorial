using FluentValidation;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(cmd => cmd.Name).Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Product name cannot be empty");
        RuleFor(cmd => cmd.CatalogBrandId).GreaterThan(0).WithMessage("Please select a product brand");
        RuleFor(cmd => cmd.CatalogTypeId).GreaterThan(0).WithMessage("Please select a product category");
        RuleFor(cmd => cmd.Price).GreaterThanOrEqualTo(0).WithMessage("Please enter product price");
        RuleFor(cmd => cmd.Stock).GreaterThanOrEqualTo(0).WithMessage("Please enter product inventory");
    }
}