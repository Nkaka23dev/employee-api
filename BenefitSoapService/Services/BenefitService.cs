
namespace BenefitSoapService.Services;

public class BenefitService : IBenefitService
{
    public string GetBenefitDetails(int id)
    {
        return $"Details for benefit ID {id}";
    }
}
