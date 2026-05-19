using AutoMapper;
using LibraryAPI.Application.DTOs.Books;
using LibraryAPI.Application.Mappings;
using LibraryAPI.Application.Services.Implementations;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;
using Moq;
using FluentAssertions;

namespace LibraryAPI.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IBookRepository> _bookRepoMock;
    private readonly IMapper _mapper;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _bookRepoMock = new Mock<IBookRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _uowMock.Setup(u => u.Books).Returns(_bookRepoMock.Object);
        _bookService = new BookService(_uowMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_DebeRetornarTodosLosLibros()
    {
        
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", ISBN = "978-0132350884", PublicationYear = 2008, Genero = new Genero { Name = "Tecnología" }, BookAuthors = new List<BookAuthor>(), Copies = new List<Copy>() },
            new Book { Id = 2, Title = "El Quijote", ISBN = "978-8491050421", PublicationYear = 1605, Genero = new Genero { Name = "Literatura" }, BookAuthors = new List<BookAuthor>(), Copies = new List<Copy>() }
        };

        _bookRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

       
        var result = await _bookService.GetAllAsync();

        
        result.Should().HaveCount(2);
        result.Should().Contain(b => b.Title == "Clean Code");
    }

    [Fact]
    public async Task GetByIdAsync_LibroExiste_DebeRetornarLibro()
    {
       
        var book = new Book
        {
            Id = 1,
            Title = "Clean Code",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            Genero = new Genero { Name = "Tecnología" },
            BookAuthors = new List<BookAuthor>(),
            Copies = new List<Copy>()
        };

        _bookRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        
        var result = await _bookService.GetByIdAsync(1);

        
        result.Should().NotBeNull();
        result!.Title.Should().Be("Clean Code");
        result.ISBN.Should().Be("978-0132350884");
    }

    [Fact]
    public async Task GetByIdAsync_LibroNoExiste_DebeRetornarNull()
    {
        
        _bookRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        
        var result = await _bookService.GetByIdAsync(99);

       
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_LibroExiste_DebeEliminarYRetornarTrue()
    {
        
        var book = new Book { Id = 1, Title = "Clean Code", ISBN = "978-0132350884", PublicationYear = 2008 };
        _bookRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);
        _bookRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        
        var result = await _bookService.DeleteAsync(1);

        
        result.Should().BeTrue();
        _bookRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_LibroNoExiste_DebeRetornarFalse()
    {
        
        _bookRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Book?)null);

        
        var result = await _bookService.DeleteAsync(99);

        
        result.Should().BeFalse();
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SearchAsync_ConFiltros_DebeRetornarLibrosFiltrados()
    {
        
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Clean Code", ISBN = "978-0132350884", PublicationYear = 2008, Genero = new Genero { Name = "Tecnología" }, BookAuthors = new List<BookAuthor>(), Copies = new List<Copy>() }
        };

        _bookRepoMock.Setup(r => r.SearchAsync("Clean", null, null, 1, 10)).ReturnsAsync(books);

        
        var result = await _bookService.SearchAsync("Clean", null, null, 1, 10);

        
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Clean Code");
    }
}