namespace SimpleAuthSystem.API.Controllers.V1
{
    [Route("SimpleAuthSystem/api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
    }
}
