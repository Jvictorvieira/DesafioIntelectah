using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class VehiclesConfiguration : IEntityTypeConfiguration<Vehicles>
{
    public void Configure(EntityTypeBuilder<Vehicles> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(v => v.VehicleId);
        builder.Property(v => v.Model)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(v => v.ManufacturingYear)
            .IsRequired();
        builder.Property(v => v.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
        builder.Property(v => v.Description)
            .HasMaxLength(500);
        builder.Property(v => v.VehicleType)
            .IsRequired();
        builder.HasMany(v => v.Sales)
            .WithOne(s => s.Vehicle)
            .HasForeignKey(s => s.VehicleId);
        builder.HasMany(v => v.Manufacturers)
            .WithMany(m => m.Vehicles);
        builder.HasIndex(v => v.Model)
            .HasDatabaseName("IX_Vehicles_Model");
    }
}