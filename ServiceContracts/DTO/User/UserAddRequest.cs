using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.User;

/// <summary>
/// DTO class for adding new User
/// </summary>
public class UserAddRequest
{
    [Required]
    [MinLength(6, ErrorMessage = "Login must be at least 6 characters long")]
    [RegularExpression("^.\\S*$", ErrorMessage = "Login cannot contain whitespaces!")]
    public string? Login { get; set; }
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public string? Password { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public Guid? CountryId { get; set; }

    [Required] 
    public bool? IsActive { get; set; } = false;

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