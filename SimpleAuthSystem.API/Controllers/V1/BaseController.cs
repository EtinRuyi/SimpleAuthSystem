using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleAuthSystem.API.Controllers.V1
{
    [Route("api/SympleAuthSystem/api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
    }
}
