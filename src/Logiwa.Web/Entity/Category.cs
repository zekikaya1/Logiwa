namespace Logiwa.Web.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinQuantity { get; set; } = 0;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    // Navigation Property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}