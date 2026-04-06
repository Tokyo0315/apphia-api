using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Apphia_Website_API.Repository.Interface;

namespace Apphia_Website_API.Controllers.Common {
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase {
        private readonly IRequestStatusHelper _requestStatusHelper;

        public CommonController(IRequestStatusHelper requestStatusHelper) {
            _requestStatusHelper = requestStatusHelper;
        }

        [HttpPost("HeartBeat")]
        public IActionResult HeartBeat() {
            return Ok(_requestStatusHelper.response(200, true, "Success", null, null));
        }

        [Authorize]
        [HttpGet("Authorize")]
        public IActionResult AuthorizeCheck() {
            var token = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token))
                return Unauthorized(_requestStatusHelper.response(401, false, "Unauthorized", null, null));
            return Ok(_requestStatusHelper.response(200, true, "Authorized", null, null));
        }
    }
}
