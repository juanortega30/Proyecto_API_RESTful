using LibraryAPI.Domain.Enums;

namespace LibraryAPI.Domain.Entities;

public class Reservation : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
}