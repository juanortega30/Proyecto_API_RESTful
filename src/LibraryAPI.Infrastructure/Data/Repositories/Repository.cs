using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LibraryContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(LibraryContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) _dbSet.Remove(entity);
    }
}