using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using LibraryAPI.Domain.Interfaces;
using LibraryAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Infrastructure.Repositories;

public class LoanRepository : Repository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryContext context) : base(context) { }

    public async Task<IEnumerable<Loan>> GetByUserIdAsync(int userId) =>
        await _context.Loans
            .Include(l => l.Copy).ThenInclude(c => c.Book)
            .Where(l => l.UserId == userId)
            .ToListAsync();

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync() =>
        await _context.Loans
            .Include(l => l.User)
            .Include(l => l.Copy).ThenInclude(c => c.Book)
            .Where(l => l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)
            .ToListAsync();

    public async Task<decimal> CalculateFineAsync(int loanId)
    {
        var loan = await _context.Loans.FindAsync(loanId);
        if (loan == null || loan.ReturnDate == null) return 0;

        var referenceDate = loan.ReturnDate.Value > loan.DueDate
            ? loan.ReturnDate.Value
            : DateTime.UtcNow;

        var daysLate = (referenceDate - loan.DueDate).Days;
        return daysLate > 0 ? daysLate * 500m : 0; 
    }
}