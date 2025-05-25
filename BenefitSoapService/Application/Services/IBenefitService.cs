using CoreWCF;
using Core.Domain.DTOs.Benefits;
using BenefitSoapService.Contacts;


namespace BenefitSoapService.Application.Services;

[ServiceContract(Namespace = "http://benefitsoapservice.com/")]
public interface IBenefitService
{
    // [OperationContract]
    // Benefit GetBenefitDetails(int benefitId);

    [OperationContract]
    Task<IEnumerable<GetBenefitResponse>> GetAllBenefits();

    [OperationContract]
    Task<BenefitContract> CreateBenefit(BenefitContract benefit);

    [OperationContract]
    Task<GetBenefitResponse> UpdateBenefit(int id, UpdateBenefit benefit);

    [OperationContract]
    Task DeleteBenefit(int benefitId);

}
