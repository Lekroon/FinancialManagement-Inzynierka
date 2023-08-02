﻿using Entities;

namespace ServiceContracts.DTO;

public class FinancialAccountResponse
{
    public Guid? AccountId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserLogin { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? AccountName { get; set; }
    public decimal? Balance { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        
        if (obj.GetType() != typeof(FinancialAccountResponse))
        {
            return false;
        }

        var financialAccountToCompare = (FinancialAccountResponse)obj;

        return AccountId == financialAccountToCompare.AccountId &&
               UserId == financialAccountToCompare.UserId &&
               CurrencyId == financialAccountToCompare.CurrencyId &&
               AccountName == financialAccountToCompare.AccountName &&
               Balance == financialAccountToCompare.Balance;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return
            $"AccountId:{AccountId}, UserId:{UserId}, CurrencyId:{CurrencyId}, " +
            $"AccountName:{AccountName}, Balance:{Balance}, CurrencyName:{CurrencyName}, UserLogin:{UserLogin}";
    }
}

public static class FinancialAccountExtensions
{
    public static FinancialAccountResponse ToFinancialAccountResponse(this FinancialAccount financialAccount)
    {
        return new FinancialAccountResponse
        {
            AccountId = financialAccount.AccountId,
            UserId = financialAccount.UserId,
            CurrencyId = financialAccount.CurrencyId,
            AccountName = financialAccount.AccountName,
            Balance = financialAccount.Balance
        };
    }
}