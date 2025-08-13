
using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CarDealershipsConfiguration : IEntityTypeConfiguration<CarDealership>
{
    public void Configure(EntityTypeBuilder<CarDealership> builder)
    {
        builder.ToTable("CarDealerships");
        builder.HasKey(cd => cd.CarDealershipId);

        builder.Property(cd => cd.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(cd => cd.Address)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(cd => cd.City)
            .HasMaxLength(50);
        builder.Property(cd => cd.State)
            .HasMaxLength(50);
        builder.Property(cd => cd.AddressCode)
            .HasMaxLength(10);
        builder.Property(cd => cd.Phone)
            .HasMaxLength(15);
        builder.Property(cd => cd.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(cd => cd.MaxVehicleCapacity)
            .IsRequired();
        builder.HasMany(cd => cd.Sales)
            .WithOne(s => s.CarDealership)
            .HasForeignKey(s => s.CarDealershipId);
        builder.HasIndex(cd => cd.Name)
            .IsUnique()
            .HasDatabaseName("IX_CarDealerships_Name");
    }
}