namespace LibraryAPI.Application.DTOs.Loans;

public class CreateLoanDto
{
    public int UserId { get; set; }
    public int CopyId { get; set; }
    public int LoanDays { get; set; } = 14;
}