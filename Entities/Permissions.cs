namespace Entities;

public class Permissions
{
    public Guid PermissionId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? AccountId { get; set; }
}