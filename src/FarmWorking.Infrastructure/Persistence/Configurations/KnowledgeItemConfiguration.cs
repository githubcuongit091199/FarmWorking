using FarmWorking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarmWorking.Infrastructure.Persistence.Configurations;

public class KnowledgeItemConfiguration : IEntityTypeConfiguration<KnowledgeItem>
{
    public void Configure(EntityTypeBuilder<KnowledgeItem> b)
    {
        b.ToTable("KnowledgeItems"); b.HasKey(x=>x.Id);
        b.Property(x=>x.Name).HasMaxLength(250).IsRequired();
        b.Property(x=>x.StoredName).HasMaxLength(100);
        b.Property(x=>x.ContentType).HasMaxLength(150);
        b.HasOne(x=>x.Parent).WithMany(x=>x.Children).HasForeignKey(x=>x.ParentId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x=>new{x.ParentId,x.Name}).IsUnique();
    }
}
