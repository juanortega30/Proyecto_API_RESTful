using AutoMapper;
using LibraryAPI.Application.DTOs.Users;
using LibraryAPI.Application.Mappings;
using LibraryAPI.Application.Services.Implementations;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using LibraryAPI.Domain.Interfaces;
using Moq;
using FluentAssertions;

namespace LibraryAPI.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IRepository<User>> _userRepoMock;
    private readonly IMapper _mapper;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _userRepoMock = new Mock<IRepository<User>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
        _userService = new UserService(_uowMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_DebeRetornarTodosLosUsuarios()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, FirstName = "Juan", LastName = "Pérez", Email = "juan@test.com", Role = UserRole.Reader },
            new User { Id = 2, FirstName = "Admin", LastName = "Sistema", Email = "admin@test.com", Role = UserRole.Admin }
        };

        _userRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Email == "juan@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_UsuarioExiste_DebeRetornarUsuario()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "Juan", LastName = "Pérez", Email = "juan@test.com", Role = UserRole.Reader };
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("juan@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_UsuarioNoExiste_DebeRetornarNull()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_UsuarioExiste_DebeEliminarYRetornarTrue()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "Juan", LastName = "Pérez", Email = "juan@test.com" };
        _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _userRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _userService.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _userRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_UsuarioNoExiste_DebeRetornarFalse()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.DeleteAsync(99);

        // Assert
        result.Should().BeFalse();
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}