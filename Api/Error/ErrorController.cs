using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Error
{
    [Route("errors/{code}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiReponse(code));
        }
    }
}
