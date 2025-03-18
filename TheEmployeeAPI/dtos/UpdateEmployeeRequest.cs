using FluentValidation;
// using TheEmployeeAPI.abstraction;

namespace TheEmployeeAPI.employees;

public class UpdateEmployeeRequest
{
    public  string? Address1 {get; set;}
    public  string? Address2 {get; set;}
    public  string? City {get; set;}
    public  string? State {get; set;}  
    public  string? ZipCode {get; set;}
    public  string? PhoneNumber {get; set;}
    public  string? Email {get; set;}

}

public class UpdateEmployeeRequestValidator: AbstractValidator<UpdateEmployeeRequest>
{
    private readonly HttpContext _httpContext;
    private readonly  AppBbContext _appDbContext;

    public UpdateEmployeeRequestValidator(IHttpContextAccessor httpContextAccessor, AppBbContext appDbContext){

        _httpContext = httpContextAccessor.HttpContext!;
        _appDbContext = appDbContext;

        RuleFor(x => x.Address1).MustAsync(NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync)
        .WithMessage("Address1 must not be empty as an address was alread set on employee");
    }

    private async Task<bool> NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync(string? address, CancellationToken token)
    {
    await Task.CompletedTask;
    var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
    var employee = await _appDbContext.Employees.FindAsync(id);

    if(employee!.Address1 != null && string.IsNullOrWhiteSpace(address)){
        return false;
    }
     return true;
    } 
};
