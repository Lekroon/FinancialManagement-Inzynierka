using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.User;

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

    public Entities.User ToUser()
    {
        return new Entities.User
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