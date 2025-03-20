using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class BaseController : Controller
{
}