using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Author> Authors { get; }
    IRepository<Genero> Genero { get; }
    IRepository<User> Users { get; }
    IRepository<Copy> Copies { get; }
    IBookRepository Books { get; }
    ILoanRepository Loans { get; }
    IRepository<Reservation> Reservations { get; }

    Task<int> SaveChangesAsync();
}
