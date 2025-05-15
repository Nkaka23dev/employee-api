using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheEmployeeAPI.Domain.DTOs.Users;
using TheEmployeeAPI.Application.User.Services;
namespace TheEmployeeAPI.WebAPI.Controllers
{
    public class UserController(IUserService userServices) : BaseController
    {
        private readonly IUserService _userServices = userServices;

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>s
        /// <returns></returns>
        [HttpGet("user/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _userServices.GetUserById(id);
            return Ok(response);
        }
        /// <summary>
        /// Get Current Logged in User
        /// </summary>
        /// <returns></returns>
        [HttpGet("current-user")]
        [Authorize]
        [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = await _userServices.GetCurrentUser();
            return Ok(response);
        }
        /// <summary>
        /// Update User
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-user/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid id, UpdatedUserRequest request)
        {
            var response = await _userServices.UpdateUser(id, request);
            return Ok(response);
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-user/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletUser(Guid id)
        {
            await _userServices.DeleteUser(id);
            return Ok();
        }
    }
}
