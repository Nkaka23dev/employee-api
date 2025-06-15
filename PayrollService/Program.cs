using Microsoft.EntityFrameworkCore;
using PayrollService.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("inMem"));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();

builder.Services.AddScoped<IPayrollRepo, PayrollRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

PrepDb.PrepPopulation(app);
app.Run();
