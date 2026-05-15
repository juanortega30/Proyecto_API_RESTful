using System;

namespace LibraryAPI.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }

    public int GeneroId { get; set; }
    public Genero Genero { get; set; } = null!;

    // Many-to-many con Author
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    // Ejemplares físicos
    public ICollection<Copy> Copies { get; set; } = new List<Copy>();
}
