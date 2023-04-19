using FluentValidation;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(cmd => cmd.Name).Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Product name cannot be empty");
        RuleFor(cmd => cmd.CatalogBrandId).NotEqual(Guid.Empty).WithMessage("Please select a product brand");
        RuleFor(cmd => cmd.CatalogTypeId)
            .NotEqual(0).WithMessage("Please select a product category")
            .GreaterThan(0).WithMessage("Product doesn't exist");
        RuleFor(cmd => cmd.Price)
            .NotEqual(0).WithMessage("Please enter product price")
            .GreaterThan(0).WithMessage("Price input error");
        RuleFor(cmd => cmd.Stock)
            .NotEqual(0).WithMessage("Please enter product inventory")
            .GreaterThan(0).WithMessage("Price input error");
    }
}