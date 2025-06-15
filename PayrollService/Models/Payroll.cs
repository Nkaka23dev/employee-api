using System.ComponentModel.DataAnnotations;

namespace PayrollService.Models;

public class Payroll
{
    [Key]
    public int Id { get; set; }
    public required string WorkLocation { get; set; }
    public required string Notes { get; set; }
    public string? ProjectCode { get; set; }
    public required string Amount { get; set; }
}
// Required means that property should be set during object construction
//[Required] means at run time an error should be thrown when the value is not provided
//Didn't set [Required] property because I think the model won't receive data directly