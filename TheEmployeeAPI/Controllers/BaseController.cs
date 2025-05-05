using Microsoft.AspNetCore.Mvc;

namespace TheEmployeeAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class BaseController : Controller
{
}