using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.Permission;

public class PermissionAddRequest
{
    [Required] 
    public Guid? UserId { get; set; }
    
    [Required] 
    public Guid? AccountId { get; set; }

    public Entities.Permission ToPermission()
    {
        return new Entities.Permission
        {
            UserId = UserId,
            AccountId = AccountId
        };
    }
}