using Entities;
using ServiceContracts.DTO.TransactionCategory;

namespace ServiceContracts;

public interface ITransactionsCategoriesService
{
    public TransactionCategoryResponse AddTransactionCategory(TransactionCategoryAddRequest? categoryAddRequest);

    public List<TransactionCategoryResponse> GetAllTransactionCategories();

    public List<TransactionCategoryResponse> GetTransactionCategoryById(Guid? categoryId);

    public TransactionCategoryResponse UpdateTransactionCategory(TransactionCategoryUpdateRequest? categoryUpdateRequest);

    public bool DeleteTransactionCategory(Guid? categoryId);
}