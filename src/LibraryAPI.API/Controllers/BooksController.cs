using LibraryAPI.Application.DTOs.Books;
using LibraryAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null) return NotFound(new { message = $"Libro con id {id} no encontrado." });
        return Ok(book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? title,
        [FromQuery] string? author,
        [FromQuery] string? genre,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var books = await _bookService.SearchAsync(title, author, genre, page, pageSize);
        return Ok(books);
    }

    [HttpGet("most-borrowed-by-category")]
    public async Task<IActionResult> GetMostBorrowedByCategory()
    {
        var books = await _bookService.GetMostBorrowedByCategoryAsync();
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var book = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
    {
        var book = await _bookService.UpdateAsync(id, dto);
        if (book == null) return NotFound(new { message = $"Libro con id {id} no encontrado." });
        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookService.DeleteAsync(id);
        if (!result) return NotFound(new { message = $"Libro con id {id} no encontrado." });
        return NoContent();
    }
}