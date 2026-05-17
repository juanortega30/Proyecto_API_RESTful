using LibraryAPI.Application.DTOs.Loans;

namespace LibraryAPI.Application.Services.Interfaces;

public interface ILoanService
{
    Task<IEnumerable<LoanDto>> GetAllAsync();
    Task<LoanDto?> GetByIdAsync(int id);
    Task<IEnumerable<LoanDto>> GetByUserIdAsync(int userId);
    Task<LoanDto> CreateAsync(CreateLoanDto dto);
    Task<LoanDto?> ReturnAsync(int loanId);
    Task<IEnumerable<LoanDto>> GetOverdueAsync();
}