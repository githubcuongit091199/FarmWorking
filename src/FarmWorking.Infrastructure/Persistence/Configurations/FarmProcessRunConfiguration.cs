using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmWorking.Infrastructure.Persistence.Configurations;

public class FarmProcessRunConfiguration : IEntityTypeConfiguration<FarmProcessRun>
{
    public void Configure(EntityTypeBuilder<FarmProcessRun> b)
    {
        b.ToTable("FarmProcessRuns"); b.HasKey(x=>x.Id);
        b.Property(x=>x.ProcessName).HasMaxLength(250).IsRequired();
        b.Property(x=>x.SeasonName).HasMaxLength(150).IsRequired();
        b.HasOne(x=>x.Farm).WithMany().HasForeignKey(x=>x.FarmId).OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x=>x.WorkProcess).WithMany().HasForeignKey(x=>x.WorkProcessId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x=>new{x.FarmId,x.StartDate});
    }
}

public class FarmProcessRunStepConfiguration : IEntityTypeConfiguration<FarmProcessRunStep>
{
    public void Configure(EntityTypeBuilder<FarmProcessRunStep> b)
    {
        b.ToTable("FarmProcessRunSteps"); b.HasKey(x=>x.Id);
        b.Property(x=>x.Title).HasMaxLength(250).IsRequired();
        b.Property(x=>x.Description).HasMaxLength(2000);
        b.HasOne(x=>x.Run).WithMany(x=>x.Steps).HasForeignKey(x=>x.FarmProcessRunId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(x=>new{x.FarmProcessRunId,x.Order}).IsUnique();
    }
}
