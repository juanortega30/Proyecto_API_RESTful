using LibraryAPI.Application.DTOs.Books;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<BookDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookDto>> SearchAsync(string? title, string? author, string? genre, int page, int pageSize);
    Task<BookDto> CreateAsync(CreateBookDto dto);
    Task<BookDto?> UpdateAsync(int id, UpdateBookDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<BookDto>> GetMostBorrowedByCategoryAsync();
}