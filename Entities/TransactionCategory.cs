using System.ComponentModel.DataAnnotations;

namespace Entities;

public class TransactionCategory
{
    [Key]
    public Guid CategoryId { get; set; }
    
    [StringLength(30)]
    public string? CategoryName { get; set; }
}