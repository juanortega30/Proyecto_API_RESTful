using AutoMapper;
using LibraryAPI.Application.DTOs.Loans;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using LibraryAPI.Domain.Interfaces;

namespace LibraryAPI.Application.Services.Implementations;

public class LoanService : ILoanService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public LoanService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDto>> GetAllAsync()
    {
        var loans = await _uow.Loans.GetAllAsync();
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }

    public async Task<LoanDto?> GetByIdAsync(int id)
    {
        var loan = await _uow.Loans.GetByIdAsync(id);
        return loan == null ? null : _mapper.Map<LoanDto>(loan);
    }

    public async Task<IEnumerable<LoanDto>> GetByUserIdAsync(int userId)
    {
        var loans = await _uow.Loans.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }

    public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
    {
        var copy = await _uow.Copies.GetByIdAsync(dto.CopyId);
        if (copy == null || !copy.IsAvailable)
            throw new InvalidOperationException("El ejemplar no está disponible.");

        var loan = new Loan
        {
            UserId = dto.UserId,
            CopyId = dto.CopyId,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(dto.LoanDays),
            Status = LoanStatus.Active
        };

        copy.IsAvailable = false;
        await _uow.Copies.UpdateAsync(copy);
        await _uow.Loans.CreateAsync(loan);
        await _uow.SaveChangesAsync();
        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<LoanDto?> ReturnAsync(int loanId)
    {
        var loan = await _uow.Loans.GetByIdAsync(loanId);
        if (loan == null) return null;

        loan.ReturnDate = DateTime.UtcNow;
        loan.Status = LoanStatus.Returned;
        loan.FineAmount = await _uow.Loans.CalculateFineAsync(loanId);

        var copy = await _uow.Copies.GetByIdAsync(loan.CopyId);
        if (copy != null)
        {
            copy.IsAvailable = true;
            await _uow.Copies.UpdateAsync(copy);
        }

        await _uow.Loans.UpdateAsync(loan);
        await _uow.SaveChangesAsync();
        return _mapper.Map<LoanDto>(loan);
    }

    public async Task<IEnumerable<LoanDto>> GetOverdueAsync()
    {
        var loans = await _uow.Loans.GetOverdueLoansAsync();
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}