using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class that is used as return type for most of UsersService methods
/// </summary>
public class UserResponse
{
    public Guid UserId { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? CountryId { get; set; }
    public bool? IsActive { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(UserResponse))
        {
            return false;
        }

        var userToCompare = (UserResponse)obj;

        return UserId == userToCompare.UserId && 
               Login == userToCompare.Login &&
               Password == userToCompare.Password &&
               Email == userToCompare.Email &&
               PhoneNumber == userToCompare.PhoneNumber &&
               CountryId == userToCompare.CountryId &&
               IsActive == userToCompare.IsActive;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        
        hashCode.Add(UserId);
        hashCode.Add(Login);
        hashCode.Add(Password);
        hashCode.Add(Email);
        hashCode.Add(PhoneNumber);
        hashCode.Add(CountryId);
        hashCode.Add(IsActive);
        
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        return
            $"UserId:{UserId}, Login:{Login}, Password:{Password}, Email:{Email}, Phone(optional):{PhoneNumber}, " +
            $"CountryId: {CountryId}, IsActive:{IsActive}";
    }
}

public static class UserExtensions
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            UserId = user.UserId,
            Login = user.Login,
            Password = user.Password,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CountryId = user.CountryId,
            IsActive = user.IsActive,
        };
    }
}