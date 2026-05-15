using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Domain.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? genre, int page, int pageSize);
    Task<IEnumerable<Book>> GetMostBorrowedByCategoryAsync();
    Task<bool> IsbnExistsAsync(string isbn);
}