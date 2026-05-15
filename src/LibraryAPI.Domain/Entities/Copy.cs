using System;

namespace LibraryAPI.Domain.Entities;

public class Copy : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public bool IsAvailable { get; set; } = true;
    public string? Condition { get; set; } // Nuevo, Bueno, Deteriorado

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}