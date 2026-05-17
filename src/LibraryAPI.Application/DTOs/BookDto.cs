namespace LibraryAPI.Application.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public string GeneroName { get; set; } = string.Empty;
    public List<string> Authors { get; set; } = new();
    public int AvailableCopies { get; set; }
}