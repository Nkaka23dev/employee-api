using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.Domain.Contracts.Auth;
using TheEmployeeAPI.Services.User;

namespace TheEmployeeAPI.Controllers;

public class AuthController: BaseController
{
    private readonly IUserServices _userServices;
    public AuthController(IUserServices userServices){
         _userServices = userServices;
    }
    /// <summary>
    /// Register 
    /// </summary>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request){
        var response  = await _userServices.RegisterHandler(request);
        return Ok(response);
    }
    /// <summary>
    /// Login 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request){
        var response = await _userServices.LoginHandler(request);
        return Ok(response);
    }

}
