using System.ComponentModel.DataAnnotations;

namespace RestaurantOrdering.Api.DTOs;

public class UpdateCategoryDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } =  string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
}