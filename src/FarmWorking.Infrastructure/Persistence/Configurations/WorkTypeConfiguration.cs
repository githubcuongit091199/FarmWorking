using FarmWorking.Domain.Entities;using Microsoft.EntityFrameworkCore;using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FarmWorking.Infrastructure.Persistence.Configurations;
public class WorkTypeConfiguration:IEntityTypeConfiguration<WorkType>{public void Configure(EntityTypeBuilder<WorkType>b){b.ToTable("WorkTypes");b.HasKey(x=>x.Id);b.Property(x=>x.Name).HasMaxLength(200).IsRequired();b.Property(x=>x.DailyRate).HasPrecision(18,2);b.HasIndex(x=>x.Name).IsUnique();}}
