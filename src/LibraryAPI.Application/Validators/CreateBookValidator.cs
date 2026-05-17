using FluentValidation;
using LibraryAPI.Application.DTOs.Books;

namespace LibraryAPI.Application.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(300).WithMessage("El título no puede superar 300 caracteres.");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("El ISBN es obligatorio.")
            .Length(10, 20).WithMessage("El ISBN debe tener entre 10 y 20 caracteres.");

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1000, DateTime.Now.Year)
            .WithMessage($"El año debe estar entre 1000 y {DateTime.Now.Year}.");

        RuleFor(x => x.GeneroId)
            .GreaterThan(0).WithMessage("Debe seleccionar un género válido.");

        RuleFor(x => x.AuthorIds)
            .NotEmpty().WithMessage("Debe incluir al menos un autor.");

        RuleFor(x => x.NumberOfCopies)
            .GreaterThan(0).WithMessage("Debe haber al menos 1 ejemplar.");
    }
}