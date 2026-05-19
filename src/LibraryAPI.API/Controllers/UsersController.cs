using LibraryAPI.Application.DTOs.Users;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound(new { message = $"Usuario con id {id} no encontrado." });
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result) return NotFound(new { message = $"Usuario con id {id} no encontrado." });
        return NoContent();
    }
}