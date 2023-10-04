namespace Entities;

public class Permission
{
    public Guid PermissionId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? AccountId { get; set; }
}