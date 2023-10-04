namespace ServiceContracts.DTO.Permission;

public class PermissionResponse
{
    public Guid PermissionId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(PermissionResponse))
            return false;

        var permissionToCompare = (PermissionResponse)obj;

        return PermissionId == permissionToCompare.PermissionId &&
               UserId == permissionToCompare.UserId &&
               AccountId == permissionToCompare.AccountId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"PermissionId:{PermissionId}, UserId:{UserId}, AccountId:{AccountId}, AccountName:{AccountName}";
    }
}

public static class PermissionExtensions
{
    public static PermissionResponse ToPermissionResponse(this Entities.Permission permission)
    {
        return new PermissionResponse
        {
            PermissionId = permission.PermissionId,
            UserId = permission.UserId,
            AccountId = permission.AccountId
        };
    }
}