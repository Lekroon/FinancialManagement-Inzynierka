using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class for adding new User
/// </summary>
public class UserAddRequest
{
    [Required]
    [MinLength(6)]
    public string? Login { get; set; }
    
    [Required]
    [MinLength(8)]
    public string? Password { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public Guid? CountryId { get; set; }
    
    [Required]
    public bool? IsActive { get; set; }

    public User ToUser()
    {
        return new User
        {
            Login = Login,
            Password = Password,
            Email = Email,
            PhoneNumber = PhoneNumber,
            CountryId = CountryId,
            IsActive = IsActive
        };
    }
}