using CoreWCF;
using Core.Domain.DTOs.Benefits;
using BenefitSoapService.Contacts;


namespace BenefitSoapService.Services;

[ServiceContract(Namespace = "http://benefitsoapservice.com/")]
public interface IBenefitService
{
    [OperationContract]
    Task<IEnumerable<GetBenefitResponse>> GetAllBenefits();

    [OperationContract]
    Task<BenefitContract> CreateBenefit(BenefitContract benefit);

    [OperationContract]
    Task<GetBenefitResponse> UpdateBenefit(int id, UpdateBenefit benefit);

    [OperationContract]
    Task DeleteBenefit(int benefitId);
}
