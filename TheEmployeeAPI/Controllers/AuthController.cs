using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.Contracts.Auth;
using TheEmployeeAPI.Contracts.User;
using TheEmployeeAPI.Services.Auth;

namespace TheEmployeeAPI.Controllers;

public class AuthController(IAuthService authServices) : BaseController
{
    private readonly IAuthService _authService = authServices;

    /// <summary>
    /// Register 
    /// </summary>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _authService.RegisterHandler(request);
        return Ok(response);
    }
    /// <summary>
    /// Login 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var response = await _authService.LoginHandler(request);
        return Ok(response);
    }

    /// <summary>
    /// Get new Access and Refresh Tokens
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await _authService.RefreshAccessToken(request);
        return Ok(response);
    }
    /// <summary>
    /// Revoke Refresh Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("revoke-refresh-token")]
    [Authorize]
    [ProducesResponseType(typeof(RevokeRefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeToken(RefreshTokenRequest request)
    {
        var response = await _authService.RevokeRefreshToken(request);
        if (response != null && response.Message == "Refresh token revoked successufully")
        {
            return Ok(response);
        }
        ;
        return BadRequest(response);
    }

}

