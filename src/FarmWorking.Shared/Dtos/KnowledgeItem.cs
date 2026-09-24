namespace FarmWorking.Shared;

public class KnowledgeItem
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsFolder { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateKnowledgeFolderRequest { public Guid? ParentId { get; set; } public string Name { get; set; } = string.Empty; }
public class RenameKnowledgeItemRequest { public string Name { get; set; } = string.Empty; }
