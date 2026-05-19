using LibraryAPI.Application.DTOs.Reservations;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);
        if (reservation == null) return NotFound(new { message = $"Reserva con id {id} no encontrada." });
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var reservation = await _reservationService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _reservationService.CancelAsync(id);
        if (!result) return NotFound(new { message = $"Reserva con id {id} no encontrada." });
        return NoContent();
    }
}