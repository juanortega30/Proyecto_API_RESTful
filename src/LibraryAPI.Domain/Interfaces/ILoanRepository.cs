using LibraryAPI.Domain.Entities;

namespace LibraryAPI.Domain.Interfaces;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<decimal> CalculateFineAsync(int loanId);
}