using FluentAssertions;
using LibraryAPI.Application.DTOs.Books;
using LibraryAPI.Application.Validators;

namespace LibraryAPI.Tests.Validators;

public class CreateBookValidatorTests
{
    private readonly CreateBookValidator _validator = new();

    [Fact]
    public void Validate_LibroValido_DebeSerValido()
    {
        var dto = new CreateBookDto
        {
            Title = "Clean Code",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            GeneroId = 1,
            AuthorIds = new List<int> { 1 },
            NumberOfCopies = 2
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_TituloVacio_DebeSerInvalido()
    {
        var dto = new CreateBookDto
        {
            Title = "",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            GeneroId = 1,
            AuthorIds = new List<int> { 1 },
            NumberOfCopies = 1
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validate_ISBNVacio_DebeSerInvalido()
    {
        var dto = new CreateBookDto
        {
            Title = "Clean Code",
            ISBN = "",
            PublicationYear = 2008,
            GeneroId = 1,
            AuthorIds = new List<int> { 1 },
            NumberOfCopies = 1
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ISBN");
    }

    [Fact]
    public void Validate_AnioFuturo_DebeSerInvalido()
    {
        var dto = new CreateBookDto
        {
            Title = "Clean Code",
            ISBN = "978-0132350884",
            PublicationYear = 2099,
            GeneroId = 1,
            AuthorIds = new List<int> { 1 },
            NumberOfCopies = 1
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PublicationYear");
    }

    [Fact]
    public void Validate_SinAutores_DebeSerInvalido()
    {
        var dto = new CreateBookDto
        {
            Title = "Clean Code",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            GeneroId = 1,
            AuthorIds = new List<int>(),
            NumberOfCopies = 1
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "AuthorIds");
    }
}