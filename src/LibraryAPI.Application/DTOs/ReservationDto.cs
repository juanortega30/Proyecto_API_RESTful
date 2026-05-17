namespace LibraryAPI.Application.DTOs.Reservations;

public class ReservationDto
{
    public int Id { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string BookTitle { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; } = string.Empty;
}