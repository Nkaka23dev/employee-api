
namespace Core.Domain.DTOs.Benefits;

public class GetBenefitResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal BaseCost { get; set; }

}
