using LibraryAPI.Application.DTOs.Loans;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _loanService.GetAllAsync();
        return Ok(loans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null) return NotFound(new { message = $"Préstamo con id {id} no encontrado." });
        return Ok(loan);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var loans = await _loanService.GetByUserIdAsync(userId);
        return Ok(loans);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        var loans = await _loanService.GetOverdueAsync();
        return Ok(loans);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoanDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var loan = await _loanService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _loanService.ReturnAsync(id);
        if (loan == null) return NotFound(new { message = $"Préstamo con id {id} no encontrado." });
        return Ok(loan);
    }
}