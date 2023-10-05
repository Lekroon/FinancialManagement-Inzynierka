﻿using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.TransactionCategory;

public class TransactionCategoryUpdateRequest
{
    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Use letters only!")]
    public string? CategoryName { get; set; }

    public Entities.TransactionCategory ToTransactionCategory()
    {
        return new Entities.TransactionCategory
        {
            CategoryId = CategoryId,
            CategoryName = CategoryName
        };
    }
}