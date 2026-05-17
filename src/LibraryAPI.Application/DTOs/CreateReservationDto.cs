namespace LibraryAPI.Application.DTOs.Reservations;

public class CreateReservationDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int ReservationDays { get; set; } = 3;
}