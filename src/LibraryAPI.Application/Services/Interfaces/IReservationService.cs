using LibraryAPI.Application.DTOs.Reservations;

namespace LibraryAPI.Application.Services.Interfaces;

public interface IReservationService
{
    Task<IEnumerable<ReservationDto>> GetAllAsync();
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<ReservationDto> CreateAsync(CreateReservationDto dto);
    Task<bool> CancelAsync(int id);
}