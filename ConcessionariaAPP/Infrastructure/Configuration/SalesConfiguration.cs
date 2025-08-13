using ConcessionariaAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SalesConfiguration : IEntityTypeConfiguration<Sales>
{
    public void Configure(EntityTypeBuilder<Sales> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.SaleId);
        builder.Property(s => s.SaleDate)
            .IsRequired();
        builder.Property(s => s.SalePrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
        builder.Property(s => s.SaleProtocol)
            .HasMaxLength(20);
        builder.HasOne(s => s.Vehicle)
            .WithMany(v => v.Sales)
            .HasForeignKey(s => s.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(s => s.Client)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(s => s.CarDealership)
            .WithMany(cd => cd.Sales)
            .HasForeignKey(s => s.CarDealershipId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(s => s.User)
            .WithMany(u => u.Sales)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}