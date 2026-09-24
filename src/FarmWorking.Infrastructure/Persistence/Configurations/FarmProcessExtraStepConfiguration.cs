using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmWorking.Infrastructure.Persistence.Configurations;

public class FarmProcessExtraStepConfiguration : IEntityTypeConfiguration<FarmProcessExtraStep>
{
    public void Configure(EntityTypeBuilder<FarmProcessExtraStep> builder)
    {
        builder.ToTable("FarmProcessExtraSteps");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(250).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.HasOne(x => x.Farm).WithMany().HasForeignKey(x => x.FarmId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.WorkProcess).WithMany().HasForeignKey(x => x.WorkProcessId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.FarmId, x.WorkProcessId, x.Order }).IsUnique();
    }
}
