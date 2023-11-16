﻿namespace Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? TransactionType { get; set; }
    public bool? IsRecurring { get; set; }
    public DateTime? RecurringDate { get; set; }
    public string? Description { get; set; }
    public bool? IsReminderSet { get; set; }
    public string? SendingMethod { get; set; }
}