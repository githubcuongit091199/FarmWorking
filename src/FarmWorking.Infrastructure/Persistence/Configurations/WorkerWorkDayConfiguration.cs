using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FarmWorking.Infrastructure.Persistence.Configurations;
public class WorkerWorkDayConfiguration:IEntityTypeConfiguration<WorkerWorkDay>{public void Configure(EntityTypeBuilder<WorkerWorkDay>b){b.ToTable("WorkerWorkDays");b.HasKey(x=>x.Id);b.Property(x=>x.WorkTypeName).HasMaxLength(200).IsRequired();b.Property(x=>x.WorkFraction).HasPrecision(5,2);b.Property(x=>x.Wage).HasPrecision(18,2);b.HasIndex(x=>new{x.WorkerId,x.WorkDate});b.HasOne(x=>x.Farm).WithMany().HasForeignKey(x=>x.FarmId).OnDelete(DeleteBehavior.Restrict);b.HasOne(x=>x.Worker).WithMany().HasForeignKey(x=>x.WorkerId).OnDelete(DeleteBehavior.Restrict);b.HasOne(x=>x.WorkType).WithMany().HasForeignKey(x=>x.WorkTypeId).OnDelete(DeleteBehavior.Restrict);}}
