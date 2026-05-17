namespace LibraryAPI.Application.DTOs.Books;

public class UpdateBookDto
{
    public string Title { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public int GeneroId { get; set; }
}