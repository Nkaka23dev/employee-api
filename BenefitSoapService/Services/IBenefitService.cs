using CoreWCF;
using BenefitSoapService.Contacts;


namespace BenefitSoapService.Services;

[ServiceContract(Namespace = "http://benefitsoapservice.com/")]
public interface IBenefitService
{
   [OperationContract]
    Benefit GetBenefitDetails(int benefitId);

    [OperationContract]
    List<Benefit> GetAllBenefits();

    [OperationContract]
    bool CreateBenefit(Benefit benefit);

    [OperationContract]
    bool UpdateBenefit(Benefit benefit);

    [OperationContract]
    bool DeleteBenefit(int benefitId); 
 
}
