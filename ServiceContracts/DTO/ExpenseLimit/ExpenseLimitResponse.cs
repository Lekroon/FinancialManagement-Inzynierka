namespace ServiceContracts.DTO.ExpenseLimit;

public class ExpenseLimitResponse
{
    public Guid LimitId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? LimitAmount { get; set; }
    public int? LimitPeriod { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(ExpenseLimitResponse))
            return false;

        var expenseLimitToCompare = (ExpenseLimitResponse)obj;

        return LimitId == expenseLimitToCompare.LimitId &&
               AccountId == expenseLimitToCompare.AccountId &&
               CategoryId == expenseLimitToCompare.CategoryId &&
               LimitAmount == expenseLimitToCompare.LimitAmount &&
               LimitPeriod == expenseLimitToCompare.LimitPeriod;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"LimitId:{LimitId}, AccountId:{AccountId}, CategoryId:{CategoryId}, LimitAmount:{LimitAmount}" +
               $", LimitPeriod:{LimitPeriod}";
    }
}

public static class ExpenseLimitExtension
{
    public static ExpenseLimitResponse ToExpenseLimitResponse(this Entities.ExpenseLimit expenseLimit)
    {
        return new ExpenseLimitResponse
        {
            LimitId = expenseLimit.LimitId,
            AccountId = expenseLimit.AccountId,
            CategoryId = expenseLimit.CategoryId,
            LimitAmount = expenseLimit.LimitAmount,
            LimitPeriod = expenseLimit.LimitPeriod
        };
    }
}