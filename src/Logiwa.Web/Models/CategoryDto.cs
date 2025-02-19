using System.ComponentModel.DataAnnotations;

namespace Logiwa.Web.Models;

public class CategoryDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int MinQuantity { get; set; }
}