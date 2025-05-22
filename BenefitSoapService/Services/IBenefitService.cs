using CoreWCF;

namespace BenefitSoapService.Services;

[ServiceContract]
public interface IBenefitService
{
    [OperationContract]
    string GetBenefitDetails(int id);
}
