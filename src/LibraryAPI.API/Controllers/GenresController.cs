using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public GenresController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var generos = await _uow.Genero.GetAllAsync();
        return Ok(generos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var genero = await _uow.Genero.GetByIdAsync(id);
        if (genero == null) return NotFound(new { message = $"Género con id {id} no encontrado." });
        return Ok(genero);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Genero genero)
    {
        await _uow.Genero.CreateAsync(genero);
        await _uow.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = genero.Id }, genero);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Genero updated)
    {
        var genero = await _uow.Genero.GetByIdAsync(id);
        if (genero == null) return NotFound(new { message = $"Género con id {id} no encontrado." });

        genero.Name = updated.Name;
        genero.Description = updated.Description;
        genero.UpdatedAt = DateTime.UtcNow;

        await _uow.Genero.UpdateAsync(genero);
        await _uow.SaveChangesAsync();
        return Ok(genero);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genero = await _uow.Genero.GetByIdAsync(id);
        if (genero == null) return NotFound(new { message = $"Género con id {id} no encontrado." });

        await _uow.Genero.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return NoContent();
    }
}