using ServiceContracts.DTO.TransactionCategory;
using Xunit.Abstractions;

namespace UnitTests;

public class TransactionCategoriesServiceTest
{
    private readonly ITransactionsCategoriesService _transactionsCategories;
    private readonly ITestOutputHelper _testOutputHelper;

    public TransactionCategoriesServiceTest(ITestOutputHelper testOutputHelper)
    {
        _transactionsCategories = new TransactionCategoriesService();
        
        
        _testOutputHelper = testOutputHelper;
    }

    #region AddTransactionCategory

    /*
     * Test requirements:
     * 1. When TransactionCategoryAddRequest is null, it should throw ArgumentNullException
     * 2. When required properties are null, it should throw ArgumentException
     * 3. When logged user want to add category with already existing name, it should throw ArgumentException
     * 4. When CategoryName contains any non-letters characters, it should throw ArgumentException
     * 5. When category is added properly, it should be added to list 
     */
    
    // 1. When TransactionCategoryAddRequest is null
    [Fact]
    public void AddTransactionCategory_CategoryAddRequestIsNull()
    {
        // Arrange
        TransactionCategoryAddRequest? categoryAddRequest = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _transactionsCategories.AddTransactionCategory(categoryAddRequest);
        });
    }
    
    // 2. When properties are null
    [Fact]
    public void AddTransactionCategory_PropertiesAreNull()
    {
        // Arrange
        var categoryAddRequest = new TransactionCategoryAddRequest
        {
            CategoryName = null
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            var response = _transactionsCategories.AddTransactionCategory(categoryAddRequest);
            _testOutputHelper.WriteLine(response.ToString());
        });
    }
    
    // 3. When category name already exists
    [Fact]
    public void AddTransactionCategory_CategoryNameAlreadyExists()
    {
        // Arrange
        var categoryToAdd1 = new TransactionCategoryAddRequest
        {
            CategoryName = "Restaurant"
        };
        var categoryToAdd2 = new TransactionCategoryAddRequest
        {
            CategoryName = "Restaurant"
        };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            var addedCategory1 = _transactionsCategories.AddTransactionCategory(categoryToAdd1);
            var addedCategory2 = _transactionsCategories.AddTransactionCategory(categoryToAdd2);
            
            _testOutputHelper.WriteLine($"Category1: {addedCategory1}");
            _testOutputHelper.WriteLine($"\n\nCategory2: {addedCategory2}");
        });
    }
    
    //4. When CategoryName contains non-letters characters
    [Fact]
    public void AddTransactionCategoty_CategoryNameContainsNonLetters()
    {
        // Arrange
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "Random12"
        };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            var addedCategory = _transactionsCategories.AddTransactionCategory(categoryToAdd);

            _testOutputHelper.WriteLine(addedCategory.ToString());
        });
    }
    
    // 5. When category is added properly
    [Fact]
    public void AddTransactionCategory_CategoryAddedProperly()
    {
        // Arrange
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "Totally cool category"
        };

        // Act
        var categoryResponse = _transactionsCategories.AddTransactionCategory(categoryToAdd);
        
        var categoryList = _transactionsCategories.GetAllTransactionCategories();
        
        // Assert
        Assert.True(categoryResponse.CategoryId != Guid.Empty);
        Assert.Contains(categoryResponse, categoryList);
    }

    #endregion
}