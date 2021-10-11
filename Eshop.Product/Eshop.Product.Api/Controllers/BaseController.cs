using Microsoft.AspNetCore.Mvc;

namespace Eshop.Product.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiversion}")]
    public class BaseController : ControllerBase
    {
    }
}
