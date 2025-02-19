using System;
using System.ComponentModel.DataAnnotations;

namespace Logiwa.Web.Models;

public class ProductDto :IValidatableObject
{
    public int Id { get; set; }

    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

  
    [MaxLength(255)] 
    public string Description { get; set; }

    [Required] 
    public int StockQuantity { get; set; }
    
    [Required]
    public int CategoryId { get; set; }

   public string? CategoryName { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Eğer Edit işlemi ise, CategoryName zorunlu değil.
        if (validationContext.ObjectInstance is ProductDto productDto)
        {
            if (productDto.Id > 0) // Edit işlemi olduğunu varsayıyoruz
            {
                // Edit işlemi sırasında CategoryName gerekmiyor
                yield break;
            }
        }

        // Create işlemi için CategoryName zorunlu
        /*if (string.IsNullOrEmpty(CategoryName))
        {
            yield return new ValidationResult("Category Name is required for Create operation.", new[] { "CategoryName" });
        }*/
    }
}