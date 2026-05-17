using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Infrastructure.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Status).HasConversion<string>();

        builder.HasOne(r => r.User)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Book)
               .WithMany()
               .HasForeignKey(r => r.BookId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}