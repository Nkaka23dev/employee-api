using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TheEmployeeAPI;

public static class Extensions
{
    public static ModelStateDictionary ToModelStateDictionary(this ValidationResult validationResult)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in validationResult.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
        return modelState;
    }

    /*
    This ToValidationProblemDetails extension is kept as a Reference when using buildin validation, so it 
    is doing nothing as we are validatin our endpoint with FluentValidator package
    // */
    // public static ValidationProblemDetails ToValidationProblemDetails(this List<ValidationResult> validationResults){
    //     var problemDetails =  new ValidationProblemDetails();
    //     foreach(var validationResult in validationResults){
    //         foreach(var memberName in validationResult.MemberNames){
    //           if(problemDetails.Errors.ContainsKey(memberName)){
    //             problemDetails.Errors[memberName] = problemDetails.Errors[memberName];

    //           }else {
    //             problemDetails.Errors[memberName] = new List<string> { validationResult.ErrorMessage! }.ToArray();
    //           }
    //         }
    //     }
    //     return problemDetails;
    //   }

}
