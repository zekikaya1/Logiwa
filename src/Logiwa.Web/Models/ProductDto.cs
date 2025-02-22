﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Logiwa.Web.Models;

public class ProductDto :IValidatableObject
{
    public int Id { get; set; }

    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
  
    public string Description { get; set; }

    [Required] 
    public int StockQuantity { get; set; }
    
    [Required]
    public int CategoryId { get; set; }

   public string? CategoryName { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is ProductDto { Id: > 0 })
        {
            yield break;
        }
    }
}