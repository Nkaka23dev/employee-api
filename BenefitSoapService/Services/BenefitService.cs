using BenefitSoapService.Contacts;
using CoreWCF;
namespace BenefitSoapService.Services;

[ServiceBehavior(Namespace = "http://benefitsoapservice.com/")]
public class BenefitService : IBenefitService
{
    private static readonly List<Benefit> Benefits =
    [
        new Benefit  { Id = 1, Name = "Health Insurance", Description = "Health Insurance", Cost = 43 },
        new Benefit { Id = 2, Name = "Retirement Plan", Description = "Health Insurance",  Cost = 654 },
        new Benefit { Id = 3, Name = "Gym Membership",  Description = "Health Insurance",  Cost = 54 },
    ];
    public Benefit GetBenefitDetails(int benefitId)
    {
        var benefit = Benefits.FirstOrDefault(b => b.Id == benefitId) ?? throw new InvalidOperationException($"Benefit with ID {benefitId} not found.");
        return benefit;
    }

    public List<Benefit> GetAllBenefits()
    {
        return Benefits.ToList();
    }

    public bool CreateBenefit(Benefit benefit)
    {
        if (Benefits.Any(b => b.Id == benefit.Id))
            return false; // already exists

        Benefits.Add(benefit);
        return true;
    }

    public bool UpdateBenefit(Benefit benefit)
    {
        var existing = Benefits.FirstOrDefault(b => b.Id == benefit.Id);
        if (existing == null)
            return false;

        existing.Name = benefit.Name;
        return true;
    }

    public bool DeleteBenefit(int benefitId)
    {
        var existing = Benefits.FirstOrDefault(b => b.Id == benefitId);
        if (existing == null)
            return false;

        Benefits.Remove(existing);
        return true;
    }
}
