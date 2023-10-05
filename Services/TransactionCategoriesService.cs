﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTO.TransactionCategory;

namespace Services;

public class TransactionCategoriesService : ITransactionsCategoriesService
{
    private readonly List<TransactionCategory> _listOfCategories;

    public TransactionCategoriesService(bool initialize = true)
    {
        _listOfCategories = new List<TransactionCategory>();

        if (initialize)
        {
            MockData();
        }
    }

    private void MockData()
    {
        _listOfCategories.AddRange(new List<TransactionCategory>
        {
            new()
            {
                CategoryId = Guid.Parse("2A423CCA-2B3A-4CEA-AB42-C792B845CB70"),
                CategoryName = "Clothes"
            },
            new()
            {
                CategoryId = Guid.Parse("05B5CDAB-3340-484A-8DEE-A25AB949DFF4"),
                CategoryName = "Tax"
            },
            new()
            {
                CategoryId = Guid.Parse("2264162C-89DE-4DFD-9357-A40C6057BD79"),
                CategoryName = "Grocery"
            },
            new()
            {
                CategoryId = Guid.Parse("32C065AE-7F84-4CEB-B649-F8FC1CD208DD"),
                CategoryName = "Landlord"
            },
            new()
            {
                CategoryId = Guid.Parse("1A61E366-4B11-48FF-8C37-2A3540D42563"),
                CategoryName = "Restaurant"
            }
        });
    }

    public TransactionCategoryResponse AddTransactionCategory(TransactionCategoryAddRequest? categoryAddRequest)
    {
        throw new NotImplementedException();
    }

    public List<TransactionCategoryResponse> GetAllTransactionCategories()
    {
        throw new NotImplementedException();
    }

    public List<TransactionCategoryResponse> GetTransactionCategoryById(Guid? categoryId)
    {
        throw new NotImplementedException();
    }

    public TransactionCategoryResponse UpdateTransactionCategory(TransactionCategoryUpdateRequest? categoryUpdateRequest)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTransactionCategory(Guid? categoryId)
    {
        throw new NotImplementedException();
    }
}