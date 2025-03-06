using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TheEmployeeAPI;

public static class Extensions
{
    public static ValidationProblemDetails ToValidationProblemDetails(this List<ValidationResult> validationResults){
        var problemDetails =  new ValidationProblemDetails();
        foreach(var validationResult in validationResults){
            foreach(var memberName in validationResult.MemberNames){
              if(problemDetails.Errors.ContainsKey(memberName)){
                problemDetails.Errors[memberName] = problemDetails.Errors[memberName];

              }else {
                problemDetails.Errors[memberName] = new List<string> { validationResult.ErrorMessage! }.ToArray();
              }
            }
        }
        return problemDetails;
     }

}
