using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Permission
{
    [Key]
    public Guid PermissionId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? AccountId { get; set; }
}