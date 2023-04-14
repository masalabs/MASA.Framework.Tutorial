using FluentValidation;

namespace Masa.EShop.Service.Catalog.Application.Catalogs.Commands;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(cmd => cmd.ProductId).NotEqual(Guid.Empty).WithMessage("Please enter the ProductId");
    }
}