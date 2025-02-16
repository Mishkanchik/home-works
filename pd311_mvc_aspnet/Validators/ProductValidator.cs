using FluentValidation;
using pd311_mvc_aspnet.Models;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Будь ласка, вкажіть назву продукту")
            .MaximumLength(100).WithMessage("Назва не може бути довшою за 100 символів")
            .Must(NotContainSpecialCharacters).WithMessage("Назва не повинна містити спеціальних символів");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Будь ласка, вкажіть опис продукту")
            .Length(20, 500).WithMessage("Опис має бути від 20 до 500 символів");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Ціна має бути більшою за 0");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Кількість не може бути від’ємною");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Будь ласка, оберіть категорію");
    }

    private bool NotContainSpecialCharacters(string name) 
    {
        return name.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
    }
}
