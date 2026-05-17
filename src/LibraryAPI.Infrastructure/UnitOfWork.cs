using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Infrastructure.Data;
using LibraryAPI.Infrastructure.Repositories;

namespace LibraryAPI.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryContext _context;

    public IRepository<Author> Authors { get; }
    public IRepository<Genero> Genero { get; }
    public IRepository<User> Users { get; }
    public IRepository<Copy> Copies { get; }
    public IBookRepository Books { get; }
    public ILoanRepository Loans { get; }
    public IRepository<Reservation> Reservations { get; }

    public UnitOfWork(LibraryContext context)
    {
        _context = context;
        Authors = new Repository<Author>(context);
        Genero = new Repository<Genero>(context);
        Users = new Repository<User>(context);
        Copies = new Repository<Copy>(context);
        Books = new BookRepository(context);
        Loans = new LoanRepository(context);
        Reservations = new Repository<Reservation>(context);
    }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}