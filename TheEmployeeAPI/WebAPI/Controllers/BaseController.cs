using Microsoft.AspNetCore.Mvc;

namespace TheEmployeeAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
    }
}