using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Common.SharedResources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net.Http.Headers;
using System.Threading;

namespace CircuitsUc.Api.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<GeneralMessages> _localization;

        public MessageController(IStringLocalizer<GeneralMessages> localization,
            IHttpContextAccessor httpContextAccessor)
        {
            _localization = localization;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("LoginFirst")]
        public GeneralResponse<string> LoginFirst()
        {
            return new GeneralResponse<string>(_localization["LoginFirstMessage"], System.Net.HttpStatusCode.Unauthorized);
        }
        [HttpGet("UserNotFound")]
        public GeneralResponse<string> UserNotFound()
        {
            return new GeneralResponse<string>(_localization["UserNotFound"], System.Net.HttpStatusCode.Unauthorized);
        }

        [HttpGet("NoPermission")]
        public GeneralResponse<string> NoPermission()
        { 
            return new GeneralResponse<string>(_localization["NoPermission"], System.Net.HttpStatusCode.Unauthorized);
        }


    

    }
}
