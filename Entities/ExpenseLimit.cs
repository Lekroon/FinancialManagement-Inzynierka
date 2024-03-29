﻿using System.ComponentModel.DataAnnotations;

namespace Entities;

public class ExpenseLimit
{
    [Key]
    public Guid LimitId { get; set; }
    
    public Guid? AccountId { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public decimal? LimitAmount { get; set; }
    
    public int? LimitPeriod { get; set; }
}