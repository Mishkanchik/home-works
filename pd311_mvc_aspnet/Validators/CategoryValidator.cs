using FluentValidation;
using pd311_mvc_aspnet.Models;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Будь ласка, вкажіть назву категорії")
            .MaximumLength(100).WithMessage("Назва не може бути довшою за 100 символів")
            .Must(NotContainSpecialCharacters).WithMessage("Назва не повинна містити спеціальних символів");
    }

    private bool NotContainSpecialCharacters(string name)
    {
        return name.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
    }
}
