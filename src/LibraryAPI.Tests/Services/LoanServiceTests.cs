using AutoMapper;
using LibraryAPI.Application.DTOs.Loans;
using LibraryAPI.Application.Mappings;
using LibraryAPI.Application.Services.Implementations;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using LibraryAPI.Domain.Interfaces;
using Moq;
using FluentAssertions;


namespace LibraryAPI.Tests.Services;

public class LoanServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ILoanRepository> _loanRepoMock;
    private readonly Mock<IRepository<Copy>> _copyRepoMock;
    private readonly IMapper _mapper;
    private readonly LoanService _loanService;

    public LoanServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _loanRepoMock = new Mock<ILoanRepository>();
        _copyRepoMock = new Mock<IRepository<Copy>>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _uowMock.Setup(u => u.Loans).Returns(_loanRepoMock.Object);
        _uowMock.Setup(u => u.Copies).Returns(_copyRepoMock.Object);

        _loanService = new LoanService(_uowMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_EjemplarDisponible_DebeCrearPrestamo()
    {
        // Arrange
        var copy = new Copy { Id = 1, BookId = 1, IsAvailable = true, Book = new Book { Title = "Clean Code" } };
        var dto = new CreateLoanDto { UserId = 1, CopyId = 1, LoanDays = 14 };

        _copyRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(copy);
        _loanRepoMock.Setup(r => r.CreateAsync(It.IsAny<Loan>())).ReturnsAsync((Loan l) => l);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _loanService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        _copyRepoMock.Verify(r => r.UpdateAsync(It.Is<Copy>(c => !c.IsAvailable)), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_EjemplarNoDisponible_DebeLanzarExcepcion()
    {
        // Arrange
        var copy = new Copy { Id = 1, IsAvailable = false };
        var dto = new CreateLoanDto { UserId = 1, CopyId = 1, LoanDays = 14 };

        _copyRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(copy);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _loanService.CreateAsync(dto));
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ReturnAsync_PrestamoExiste_DebeRegistrarDevolucion()
    {
        // Arrange
        var loan = new Loan
        {
            Id = 1,
            CopyId = 1,
            Status = LoanStatus.Active,
            DueDate = DateTime.UtcNow.AddDays(5),
            User = new User { FirstName = "Juan", LastName = "Pérez" },
            Copy = new Copy { Book = new Book { Title = "Clean Code" } }
        };
        var copy = new Copy { Id = 1, IsAvailable = false };

        _loanRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(loan);
        _loanRepoMock.Setup(r => r.CalculateFineAsync(1)).ReturnsAsync(0);
        _copyRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(copy);
        _loanRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Loan>())).ReturnsAsync((Loan l) => l);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _loanService.ReturnAsync(1);

        // Assert
        result.Should().NotBeNull();
        loan.Status.Should().Be(LoanStatus.Returned);
        loan.ReturnDate.Should().NotBeNull();
        _copyRepoMock.Verify(r => r.UpdateAsync(It.Is<Copy>(c => c.IsAvailable)), Times.Once);
    }

    [Fact]
    public async Task GetOverdueAsync_DebeRetornarPrestamosvencidos()
    {
        // Arrange
        var overdueLoans = new List<Loan>
        {
            new Loan
            {
                Id = 1, Status = LoanStatus.Active,
                DueDate = DateTime.UtcNow.AddDays(-3),
                User = new User { FirstName = "Juan", LastName = "Pérez" },
                Copy = new Copy { Book = new Book { Title = "Clean Code" } }
            }
        };

        _loanRepoMock.Setup(r => r.GetOverdueLoansAsync()).ReturnsAsync(overdueLoans);

        // Act
        var result = await _loanService.GetOverdueAsync();

        // Assert
        result.Should().HaveCount(1);
    }
}