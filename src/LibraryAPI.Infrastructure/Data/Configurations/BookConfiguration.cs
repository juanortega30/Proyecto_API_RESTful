using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(300);
        builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
        builder.HasIndex(b => b.ISBN).IsUnique();
        builder.Property(b => b.PublicationYear).IsRequired();

        builder.HasOne(b => b.Genero)
               .WithMany(g => g.Books)
               .HasForeignKey(b => b.GeneroId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.BookAuthors)
               .WithOne(ba => ba.Book)
               .HasForeignKey(ba => ba.BookId);
    }
}