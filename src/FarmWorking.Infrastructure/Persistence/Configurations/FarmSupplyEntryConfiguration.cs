using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FarmWorking.Infrastructure.Persistence.Configurations;
public class FarmSupplyEntryConfiguration:IEntityTypeConfiguration<FarmSupplyEntry>{public void Configure(EntityTypeBuilder<FarmSupplyEntry>b){b.ToTable("FarmSupplyEntries");b.HasKey(x=>x.Id);b.Property(x=>x.UnitPrice).HasPrecision(18,2);b.Property(x=>x.Quantity).HasPrecision(18,2);b.Property(x=>x.UsedQuantity).HasPrecision(18,2);b.HasOne(x=>x.Farm).WithMany().HasForeignKey(x=>x.FarmId).OnDelete(DeleteBehavior.Cascade);b.HasOne(x=>x.Supply).WithMany().HasForeignKey(x=>x.SupplyId).OnDelete(DeleteBehavior.Restrict);b.HasOne(x=>x.SupplyPrice).WithMany().HasForeignKey(x=>x.SupplyPriceId).OnDelete(DeleteBehavior.Restrict);b.HasIndex(x=>new{x.FarmId,x.SupplyId,x.ReceivedAt});}}
