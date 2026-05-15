using LibraryAPI.Domain.Enums;

namespace LibraryAPI.Domain.Entities;

public class Loan : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int CopyId { get; set; }
    public Copy Copy { get; set; } = null!;

    public DateTime LoanDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public LoanStatus Status { get; set; } = LoanStatus.Active;

    public decimal FineAmount { get; set; } = 0;
    public bool FinePaid { get; set; } = false;
}