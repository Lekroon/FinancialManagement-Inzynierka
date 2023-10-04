using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.TransactionCategory;

public class TransactionCategoryAddRequest
{
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Use letters only!")]
    public string? CategoryName { get; set; }

    public Entities.TransactionCategory ToTransactionCategory()
    {
        return new Entities.TransactionCategory
        {
            CategoryName = CategoryName
        };
    }
}