using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.User;

public class UserUpdateRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string? Password { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    public Entities.User ToUser()
    {
        return new Entities.User
        {
            UserId = UserId,
            Password = Password,
            Email = Email,
            PhoneNumber = PhoneNumber
        };
    }
}