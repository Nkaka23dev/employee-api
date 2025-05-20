using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.Application.Employees.Services;
using TheEmployeeAPI.Domain.DTOs.Employees;

namespace TheEmployeeAPI.WebAPI.Controllers
{
    public class EmployeeController(
     ILogger<EmployeeController> logger,
     IEmployeeService employeeService) : BaseController
    {
        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly IEmployeeService _employeeService = employeeService;

        /// <summary>
        /// Get All Employees
        /// </summary>
        /// <param name="request"></param>s
        /// <returns>Returns the employees in JSON array.</returns>
        [HttpGet("all")]
        // [Authorize]
        [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllEmployees([FromQuery] GetAllEmployeesRequest request)
        {
            var response = await _employeeService.GetAllEmployeesAsync(request);
            return Ok(response);
        }
        /// <summary>
        /// Get Employee by Id.
        /// </summary> 
        /// <param name="id">ID of an employee you want to get</param>
        /// <returns>Return employee object</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var response = await _employeeService.GetEmployeeAsync(id);
            return Ok(response);
        }
        /// <summary>
        ///  Create new Employees
        /// </summary>
        /// <param name="request">Object containing required field to create employee</param>
        /// <returns>Return 201 created</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request)
        {
            var response = await _employeeService.CreateEmployeeAsync(request);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = response.Id }, response);
        }
        /// <summary>
        /// Update Employee
        /// </summary> 
        /// <param name="id">The Id of an Employee to Update.</param>
        /// <param name="request">The Employee data to update</param>
        /// <returns>Return Updated Employee</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest request)
        {
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, request);
                _logger.LogInformation("Employee with ID: {employeeID} successfully updated", id);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with ID {employeeID}", id);
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }
        /// <summary>
        /// Delete Employee
        /// </summary>
        /// <param name="id">Id of an employee to be deleted</param>
        /// <returns>No content for deleted employee</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
        /// <summary>
        /// Gets the benefits for employee.
        /// </summary>
        /// <param name="employeeId">The ID to get the benefits for.</param>
        /// <returns>The benefits for that employee.</returns>
        [HttpGet("{employeeId}/benefits")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefits>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBenefitsForEmployee(int employeeId)
        {
            var benefits = await _employeeService.GetBenefitsForEmployeeAsync(employeeId);
            return Ok(benefits);
        }
    }
}
