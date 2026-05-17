using AutoMapper;
using LibraryAPI.Application.DTOs.Reservations;
using LibraryAPI.Application.Services.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using LibraryAPI.Domain.Interfaces;

namespace LibraryAPI.Application.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ReservationService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        var reservations = await _uow.Reservations.GetAllAsync();
        return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _uow.Reservations.GetByIdAsync(id);
        return reservation == null ? null : _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        var reservation = new Reservation
        {
            UserId = dto.UserId,
            BookId = dto.BookId,
            ReservationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(dto.ReservationDays),
            Status = ReservationStatus.Pending
        };

        await _uow.Reservations.CreateAsync(reservation);
        await _uow.SaveChangesAsync();
        return _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<bool> CancelAsync(int id)
    {
        var reservation = await _uow.Reservations.GetByIdAsync(id);
        if (reservation == null) return false;

        reservation.Status = ReservationStatus.Cancelled;
        await _uow.Reservations.UpdateAsync(reservation);
        await _uow.SaveChangesAsync();
        return true;
    }
}