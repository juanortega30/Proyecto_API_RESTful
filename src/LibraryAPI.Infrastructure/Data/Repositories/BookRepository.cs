using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Infrastructure.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryContext context) : base(context) { }

    public async Task<IEnumerable<Book>> SearchAsync(
        string? title, string? author, string? genero, int page, int pageSize)
    {
        var query = _context.Books
            .Include(b => b.Genero)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Copies)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.ToLower().Contains(title.ToLower()));

        if (!string.IsNullOrWhiteSpace(genero))
            query = query.Where(b => b.Genero.Name.ToLower().Contains(genero.ToLower()));

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.BookAuthors.Any(ba =>
                (ba.Author.FirstName + " " + ba.Author.LastName)
                .ToLower().Contains(author.ToLower())));

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetMostBorrowedByCategoryAsync()
    {
        return await _context.Books
            .Include(b => b.Genero)
            .Include(b => b.Copies).ThenInclude(c => c.Loans)
            .GroupBy(b => b.GeneroId)
            .Select(g => g.OrderByDescending(b =>
                b.Copies.SelectMany(c => c.Loans).Count()).First())
            .ToListAsync();
    }

    public async Task<bool> IsbnExistsAsync(string isbn) =>
        await _context.Books.AnyAsync(b => b.ISBN == isbn);
}