namespace Core.Domain.DTOs.Benefits;

public class CreateBenefit
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal BaseCost { get; set; }
}
