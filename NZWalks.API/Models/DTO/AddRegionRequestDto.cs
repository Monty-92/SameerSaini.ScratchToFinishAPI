using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class AddRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code must be 3 characters long")]
    [MaxLength(3, ErrorMessage = "Code must be 3 characters long")]
    public string Code { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "Name must be less than or equal to 100 characters.")]
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}