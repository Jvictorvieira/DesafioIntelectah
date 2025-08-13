using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UsersConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.UserId);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(u => u.AccessLevel)
            .IsRequired();

        builder.HasMany(u => u.Sales)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(u => u.Name)
            .IsUnique()
            .HasDatabaseName("IX_Users_Name");
    }
}