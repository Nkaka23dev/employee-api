using BenefitSoapService.Services;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;


var builder = WebApplication.CreateBuilder(args);

// Add CoreWCF services
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata(); 

builder.Services.AddSingleton<IBenefitService, BenefitService>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<BenefitService>();
    serviceBuilder.AddServiceEndpoint<BenefitService, IBenefitService>(
        new BasicHttpBinding(),
        "/BenefitService.svc"
    );
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
// dotnet-svcutil http://localhost:5079/BenefitService.svc\?singleWsdl -n "*,BenefitSoapService.Client"