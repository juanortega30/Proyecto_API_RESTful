namespace LibraryAPI.Application.DTOs.Loans;

public class LoanDto
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string BookTitle { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal FineAmount { get; set; }
}