using FluentAssertions;
using LibraryAPI.Application.DTOs.Users;
using LibraryAPI.Application.Validators;

namespace LibraryAPI.Tests.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Validate_UsuarioValido_DebeSerValido()
    {
        var dto = new CreateUserDto
        {
            FirstName = "Juan",
            LastName = "Pérez",
            Email = "juan@test.com",
            Password = "123456"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmailInvalido_DebeSerInvalido()
    {
        var dto = new CreateUserDto
        {
            FirstName = "Juan",
            LastName = "Pérez",
            Email = "esto-no-es-un-email",
            Password = "123456"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_PasswordCorta_DebeSerInvalido()
    {
        var dto = new CreateUserDto
        {
            FirstName = "Juan",
            LastName = "Pérez",
            Email = "juan@test.com",
            Password = "123"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validate_NombreVacio_DebeSerInvalido()
    {
        var dto = new CreateUserDto
        {
            FirstName = "",
            LastName = "Pérez",
            Email = "juan@test.com",
            Password = "123456"
        };

        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
    }
}