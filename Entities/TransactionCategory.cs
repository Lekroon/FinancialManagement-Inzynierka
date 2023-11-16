using System.ComponentModel.DataAnnotations;

namespace Entities;

public class TransactionCategory
{
    [Key]
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
}