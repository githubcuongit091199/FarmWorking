namespace FarmWorking.Domain.Entities;

public class KnowledgeItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? ParentId { get; set; }
    public bool IsFolder { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StoredName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public KnowledgeItem? Parent { get; set; }
    public ICollection<KnowledgeItem> Children { get; set; } = new List<KnowledgeItem>();
}
