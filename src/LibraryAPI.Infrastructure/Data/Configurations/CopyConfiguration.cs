using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Infrastructure.Data.Configurations;

public class CopyConfiguration : IEntityTypeConfiguration<Copy>
{
    public void Configure(EntityTypeBuilder<Copy> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Condition).HasMaxLength(50);

        builder.HasOne(c => c.Book)
               .WithMany(b => b.Copies)
               .HasForeignKey(c => c.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}