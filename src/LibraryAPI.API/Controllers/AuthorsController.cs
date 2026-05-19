using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public AuthorsController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var authors = await _uow.Authors.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null) return NotFound(new { message = $"Autor con id {id} no encontrado." });
        return Ok(author);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Author author)
    {
        await _uow.Authors.CreateAsync(author);
        await _uow.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Author updated)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null) return NotFound(new { message = $"Autor con id {id} no encontrado." });

        author.FirstName = updated.FirstName;
        author.LastName = updated.LastName;
        author.Biography = updated.Biography;
        author.UpdatedAt = DateTime.UtcNow;

        await _uow.Authors.UpdateAsync(author);
        await _uow.SaveChangesAsync();
        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _uow.Authors.GetByIdAsync(id);
        if (author == null) return NotFound(new { message = $"Autor con id {id} no encontrado." });

        await _uow.Authors.DeleteAsync(id);
        await _uow.SaveChangesAsync();
        return NoContent();
    }
}