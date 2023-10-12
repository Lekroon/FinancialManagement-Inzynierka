using ServiceContracts.DTO.TransactionCategory;
using Xunit.Abstractions;

namespace UnitTests;

public class TransactionCategoriesServiceTest
{
    private readonly ITransactionsCategoriesService _transactionsCategories;
    private readonly ITestOutputHelper _testOutputHelper;

    public TransactionCategoriesServiceTest(ITestOutputHelper testOutputHelper)
    {
        _transactionsCategories = new TransactionCategoriesService(false);
        
        
        _testOutputHelper = testOutputHelper;
    }

    #region AddTransactionCategory

    /*
     * Test requirements:
     * 1. When TransactionCategoryAddRequest is null, it should throw ArgumentNullException
     * 2. When required properties are null, it should throw ArgumentException
     * 3. When CategoryName is duplicated, it should throw ArgumentException
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
        _testOutputHelper.WriteLine($"Added category: {categoryResponse}");
        
        var categoryList = _transactionsCategories.GetAllTransactionCategories();
        
        // Assert
        Assert.True(categoryResponse.CategoryId != Guid.Empty);
        Assert.Contains(categoryResponse, categoryList);
    }

    #endregion

    #region GetAllTransactionCategories
    
    /*
     * Test requirements:
     * 1. Without adding any categories, list should be empty by default
     * 2. It should return every properly added category
     */

    [Fact]
    public void GetAllTransactionCategories_ListShouldBeEmpty()
    {
        // Acts
        var responseList = _transactionsCategories.GetAllTransactionCategories();
        
        // Assert
        Assert.Empty(responseList);
    }

    [Fact]
    public void GetAllTransactionCategories_ReturnAddedCategory()
    {
        var categoryToAdd1 = new TransactionCategoryAddRequest { CategoryName = "Something something"};
        var categoryToAdd2 = new TransactionCategoryAddRequest { CategoryName = "Random Category"};
        var categoryToAdd3 = new TransactionCategoryAddRequest { CategoryName = "Oooh aaah bomba"};

        var categoryResponse1 = _transactionsCategories.AddTransactionCategory(categoryToAdd1);
        _testOutputHelper.WriteLine("Created category #1:\n" + categoryResponse1);

        var categoryResponse2 = _transactionsCategories.AddTransactionCategory(categoryToAdd2);
        _testOutputHelper.WriteLine("Created category #2:\n" + categoryResponse2);

        var categoryResponse3 = _transactionsCategories.AddTransactionCategory(categoryToAdd3);
        _testOutputHelper.WriteLine("Created category #3:\n" + categoryResponse3);

        var categoriesList = _transactionsCategories.GetAllTransactionCategories();
        
        _testOutputHelper.WriteLine("\n\nCategory list:");
        foreach (var category in categoriesList)
        {
            _testOutputHelper.WriteLine(category.ToString());
        }
        
        foreach (var expectedCategory in categoriesList)
        {
            Assert.Contains(expectedCategory, categoriesList);
        }
    }
    
    #endregion

    #region GetTransactionCategoryById

    /*
     * Test requirements:
     * 1. If given id is null, it should return null
     * 2. If id is given propely, it should return matching category
     */

    [Fact]
    public void GetTransactionCategoryById_IdIsNull()
    {
        Guid? id = null;

        var foundCategory = _transactionsCategories.GetTransactionCategoryById(id);
        
        Assert.Null(foundCategory);
    }

    [Fact]
    public void GetTransactionCategoryById_IdGivenProperly()
    {
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "Abcdef"
        };

        var addedCategory = _transactionsCategories.AddTransactionCategory(categoryToAdd);

        var foundCategory = _transactionsCategories.GetTransactionCategoryById(addedCategory.CategoryId);

        Assert.Equal(addedCategory, foundCategory);
    }
    
    #endregion

    #region UpdateTransactionCategory

    /*
     * Test requirements:
     * 1. When TransactionCategoryUpdateRequest is null, it should throw ArgumentNullException
     * 2. When CategoryId is null or invalid, it should throw ArgumentException
     * 3. When CategoryName is null or invalid, it should throw ArgumentException
     * 4. When values are given properly, category should be updated
     */

    // 1. TransactionCategoryUpdateRequest is null
    [Fact]
    public void UpdateTransactionCategory_UpdateRequestIsNull()
    {
        TransactionCategoryUpdateRequest? categoryUpdateRequest = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            _transactionsCategories.UpdateTransactionCategory(categoryUpdateRequest);
        });
    }

    // 2. CategoryId is null or invalid
    [Fact]
    public void UpdateTransactionCategory_CategoryIdIsNull()
    {
        var categoryToUpdate = new TransactionCategoryUpdateRequest
        {
            CategoryId = Guid.NewGuid(),
            CategoryName = "Fifarafa"
        };

        Assert.Throws<ArgumentException>(() =>
        {
            _transactionsCategories.UpdateTransactionCategory(categoryToUpdate);
        });
    }

    // 3. CategoryName is null or invalid
    [Fact]
    public void UpdateTransactionCategory_CategoryNameIsNullOrInvalid()
    {
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "Blablabla"
        };

        var addedCategory = _transactionsCategories.AddTransactionCategory(categoryToAdd);
        
        _testOutputHelper.WriteLine($"Created category:\n{addedCategory}");

        // Updating category
        var categoryUpdateRequest = addedCategory.ToTransactionCategoryUpdateRequest();
        
        // Non-literals characters
        categoryUpdateRequest.CategoryName = "Abc1234";
        _testOutputHelper.WriteLine($"\n\nUpdate request:\n{categoryUpdateRequest.CategoryName}");

        Assert.Throws<ArgumentException>(() =>
        {
            _transactionsCategories.UpdateTransactionCategory(categoryUpdateRequest);
        });
    }
    
    // 4. Values are given properly
    [Fact]
    public void UpdateTransactionCategory_ValuesAreGivenProperly()
    {
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "Bombito"
        };

        var addedCategory = _transactionsCategories.AddTransactionCategory(categoryToAdd);
        _testOutputHelper.WriteLine($"Created category:\n{addedCategory}");
        
        var categoryToUpdate = addedCategory.ToTransactionCategoryUpdateRequest();
        categoryToUpdate.CategoryName = "UpdatedName";
        _testOutputHelper.WriteLine($"\n\nCatgoryToUpdate:\n{categoryToUpdate.CategoryName}");
        
        var updatedCategoryResponse = _transactionsCategories.UpdateTransactionCategory(categoryToUpdate);
        _testOutputHelper.WriteLine($"\n\nUpdated category:\n{updatedCategoryResponse}");
        
        var categoryFromGet = _transactionsCategories.GetTransactionCategoryById(updatedCategoryResponse.CategoryId);

        Assert.Equal(categoryFromGet, updatedCategoryResponse);
    }
    
    #endregion

    #region DeleteTransactionCategory

    /*
     * Test requirements:
     * 1. If given id is invalid, it should return false
     * 2. If given id is valid, it should return true and delete existing financial account
     */
    
    //1. Given id is invalid
    [Fact]
    public void DeleteTransactionCategory_InvalidId()
    {
        var id = Guid.NewGuid();

        var isDeleted = _transactionsCategories.DeleteTransactionCategory(id);
        
        Assert.False(isDeleted);
    }
    
    // 2. Given id is valid
    [Fact]
    public void DeleteTransactionCategory_IdIsValid()
    {
        var categoryToAdd = new TransactionCategoryAddRequest
        {
            CategoryName = "ToDelete"
        };

        var addedCategory = _transactionsCategories.AddTransactionCategory(categoryToAdd);

        var isDeleted = _transactionsCategories.DeleteTransactionCategory(addedCategory.CategoryId);
        
        Assert.True(isDeleted);
    }
    
    #endregion
}