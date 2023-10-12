namespace ServiceContracts.DTO.TransactionCategory;

public class TransactionCategoryResponse
{
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(TransactionCategoryResponse))
        {
            return false;
        }

        var categoryToCompare = (TransactionCategoryResponse)obj;

        return CategoryId == categoryToCompare.CategoryId &&
               CategoryName == categoryToCompare.CategoryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"CategoryId:{CategoryId}, CategoryName:{CategoryName}";
    }

    public TransactionCategoryUpdateRequest ToTransactionCategoryUpdateRequest()
    {
        return new TransactionCategoryUpdateRequest
        {
            CategoryId = CategoryId,
            CategoryName = CategoryName
        };
    }
}

public static class TransactionCategoryExtension
{
    public static TransactionCategoryResponse ToTransactionCategoryResponse(this Entities.TransactionCategory category)
    {
        return new TransactionCategoryResponse
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName
        };
    }
}