using System.Runtime.Serialization;
namespace BenefitSoapService.Contacts;

[DataContract(Namespace = "http://benefitsoapservice.com/")]
public class Benefit
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public string? Description {get; set;}

    [DataMember]
    public decimal Cost { get; set; }

}
