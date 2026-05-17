using AutoMapper;
using LibraryAPI.Application.DTOs.Books;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;

namespace LibraryAPI.Application.Services.Implementations;

public class BookService : IBookService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public BookService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _uow.Books.GetAllAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    public async Task<IEnumerable<BookDto>> SearchAsync(
        string? title, string? author, string? genre, int page, int pageSize)
    {
        var books = await _uow.Books.SearchAsync(title, author, genre, page, pageSize);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto> CreateAsync(CreateBookDto dto)
    {
        var book = _mapper.Map<Book>(dto);

        foreach (var authorId in dto.AuthorIds)
            book.BookAuthors.Add(new BookAuthor { AuthorId = authorId });

        for (int i = 0; i < dto.NumberOfCopies; i++)
            book.Copies.Add(new Copy { IsAvailable = true });

        await _uow.Books.CreateAsync(book);
        await _uow.SaveChangesAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        if (book == null) return null;

        book.Title = dto.Title;
        book.PublicationYear = dto.PublicationYear;
        book.Description = dto.Description;
        book.GeneroId = dto.GeneroId;
        book.UpdatedAt = DateTime.UtcNow;

        await _uow.Books.UpdateAsync(book);
        await _uow.SaveChangesAsync();
        return _mapper.Map<BookDto>(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _uow.Books.GetByIdAsync(id);
        if (book == null) return false;

        await _uow.Books.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<BookDto>> GetMostBorrowedByCategoryAsync()
    {
        var books = await _uow.Books.GetMostBorrowedByCategoryAsync();
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}