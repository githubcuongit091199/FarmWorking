using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FarmWorking.Infrastructure.Persistence.Configurations;
public class SupplyPriceConfiguration:IEntityTypeConfiguration<SupplyPrice>{public void Configure(EntityTypeBuilder<SupplyPrice>b){b.ToTable("SupplyPrices");b.HasKey(x=>x.Id);b.Property(x=>x.Price).HasPrecision(18,2);b.HasIndex(x=>new{x.SupplyId,x.Price}).IsUnique();b.HasOne(x=>x.Supply).WithMany().HasForeignKey(x=>x.SupplyId).OnDelete(DeleteBehavior.Cascade);}}
