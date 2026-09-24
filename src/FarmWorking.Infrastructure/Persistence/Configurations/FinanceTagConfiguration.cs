using FarmWorking.Domain.Entities; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FarmWorking.Infrastructure.Persistence.Configurations;
public class FinanceTagConfiguration:IEntityTypeConfiguration<FinanceTag>{public void Configure(EntityTypeBuilder<FinanceTag>b){b.ToTable("FinanceTags");b.HasKey(x=>x.Id);b.Property(x=>x.Name).HasMaxLength(100).IsRequired();b.HasIndex(x=>new{x.Name,x.Type}).IsUnique();}}
