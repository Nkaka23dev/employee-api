using Core.Domain.DTOs.Benefits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceReference;
using TheEmployeeAPI.Domain.DTOs.Authentication;

namespace TheEmployeeAPI.WebAPI.Controllers;

public class BenefitController : BaseController
{
    private readonly BenefitServiceClient _soapClient;
    public BenefitController()
    {
        // Initialize SOAP client with generated endpoint configuration
        _soapClient = new BenefitServiceClient(BenefitServiceClient.EndpointConfiguration.BasicHttpBinding_IBenefitService);
    }
    /// <summary>
    /// Get all Benefits
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<BenefitContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllBenefits()
    {
        var response = await _soapClient.GetAllBenefitsAsync();
        return Ok(response);
    }
    /// <summary>
    /// Add New Benefit
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(BenefitContract), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBenefit([FromBody] BenefitContract request)
    {
        var response = await _soapClient.CreateBenefitAsync(request);
        return CreatedAtAction(nameof(GetAllBenefits), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update Employee benefits
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns>Return Updated Benefit</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(UpdateBenefit), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateBenefit(int id, [FromBody] UpdateBenefit request)
    {
        var response = await _soapClient.UpdateBenefitAsync(id, request);
        return Ok(response);
    }

    /// <summary>
    /// Delete Benefit 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>This endpoint return no content found</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBenefit(int id)
    {
        await _soapClient.DeleteBenefitAsync(id);
        return NoContent();
    }
}
