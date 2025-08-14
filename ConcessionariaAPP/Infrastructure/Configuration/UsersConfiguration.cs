using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UsersConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.ToTable("Users");   
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(u => u.AccessLevel)
            .IsRequired();

        builder.HasIndex(u => u.Name)
            .IsUnique()
            .HasDatabaseName("IX_Users_Name");
    }
}