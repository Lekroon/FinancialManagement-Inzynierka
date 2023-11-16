using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.User;

public class UserUpdateRequest
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [MaxLength(50)]
    public string? Password { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [Phone]
    [MaxLength(20)]
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