using BenefitSoapService.MappingProfiles;
using BenefitSoapService.Services;
using Core.Infrastructure.Repositories;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;
using TheEmployeeAPI.Infrastructure.DbContexts;


var builder = WebApplication.CreateBuilder(args);

// Add CoreWCF services
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();

var conneString = builder.Configuration.GetConnectionString("Default Connection");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(conneString));
builder.Services.AddSingleton<ISystemClock, SystemClock>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<BenefitService>();
builder.Services.AddScoped<IRepository<Benefit>, BenefitRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfiles)); 


var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<BenefitService>(
        serviceOptions =>
    {
        serviceOptions.DebugBehavior.IncludeExceptionDetailInFaults = true;
    }
    );
    serviceBuilder.AddServiceEndpoint<BenefitService, IBenefitService>(
        new BasicHttpBinding(),
        "/BenefitService.svc"
    );
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.Run();
// dotnet-svcutil http://localhost:5079/BenefitService.svc\?singleWsdl -n "*,BenefitSoapService.Client"