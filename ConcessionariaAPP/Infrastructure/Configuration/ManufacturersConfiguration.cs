using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ManufacturersConfiguration : IEntityTypeConfiguration<Manufacturers>
{
    public void Configure(EntityTypeBuilder<Manufacturers> builder)
    {
        builder.ToTable("Manufacturers");
        builder.HasKey(m => m.ManufacturerId);
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(m => m.Country)
            .HasMaxLength(50);
        builder.Property(m => m.FundationYear)
            .IsRequired();
        builder.Property(m => m.WebSite)
            .HasMaxLength(255);
        builder.HasMany(m => m.Vehicles)
            .WithMany(v => v.Manufacturers);
    }
}