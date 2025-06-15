using System.ComponentModel.DataAnnotations;

namespace PayrollService.DTOs;

public class PayrollCreateDTO
{
    [Required]
    public required string WorkLocation { get; set; }
    [Required]
    public required string Notes { get; set; }
    public string? ProjectCode { get; set; }
    [Required]
    public required string Amount { get; set; }
}
