using LibraryAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryAPI.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Status).HasConversion<string>();
        builder.Property(l => l.FineAmount).HasColumnType("decimal(10,2)");

        builder.HasOne(l => l.User)
               .WithMany(u => u.Loans)
               .HasForeignKey(l => l.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Copy)
               .WithMany(c => c.Loans)
               .HasForeignKey(l => l.CopyId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}