namespace Entities;

/// <summary>
/// Domain model used for storing User details
/// </summary>
public class User
{
    public Guid UserId { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? CountryId { get; set; }
    public bool? IsActive { get; set; }
}