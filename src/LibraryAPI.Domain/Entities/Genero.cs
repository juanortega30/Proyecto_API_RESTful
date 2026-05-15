using System;

namespace LibraryAPI.Domain.Entities;

public class Genero : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
