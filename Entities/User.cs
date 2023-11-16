using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Domain model used for storing User details
/// </summary>
public class User
{
    [Key]
    public Guid UserId { get; set; }
    
    [StringLength(50)]
    public string? Login { get; set; }
    
    [StringLength(50)]
    public string? Password { get; set; }
    
    [StringLength(100)]
    public string? Email { get; set; }
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    public Guid? CountryId { get; set; }
    
    public bool? IsActive { get; set; }
}