
using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.IServices;
using CircuitsUc.Application.Models.AuthDTO;
using CircuitsUc.Application.Services;
using CircuitsUc.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace CircuitsUc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        private readonly IStringLocalizer<GeneralMessages> _localization;
        public AuthenticationController(IAuthService authenticationService, IStringLocalizer<GeneralMessages> localization)
        {
            _authenticationService = authenticationService;
            _localization = localization;
        }

        [HttpPost("SignIn")]
        public async Task<GeneralResponse<AuthResponse>> Login(AuthRequest request)
        {

            
            //var password = WebUiUtility.Decrypt("bRfSm3N1FM9ZSj3VebIS8A,,");
            return await _authenticationService.Login(request);
        }

        [HttpPost("SignUp")]
        public async Task<GeneralResponse<RegistrationResponse>> Register(RegistrationRequest request)
        {

            return await _authenticationService.Register(request);
        }

        [HttpPost("ChangePassword")]
        public async Task<GeneralResponse<ChangeUserPasswordResponse>> ChangePassword(ChangeUserPasswordRequest request)
        {
            return await _authenticationService.ChangePassword(request);
        }
       

       

       
        [HttpPost("Logout")]
        public async Task<GeneralResponse<bool>> Logout()
        {
          var UserID =   HttpContext.GetUserId();
            if (string.IsNullOrEmpty(UserID))
            {
                return new GeneralResponse<bool>(true, _localization["LogoutSuccessfully"]);

            }

            var  _CurrentUserID  = Guid.Parse(UserID);
            var result = await _authenticationService.Logout(_CurrentUserID);
            if (result)
            {
                HttpContext.Session.Clear();
                return new GeneralResponse<bool>(true, _localization["LogoutSuccessfully"]);
            }
            return new GeneralResponse<bool>(_localization["ErrorInLogout"], System.Net.HttpStatusCode.BadRequest);
        }
       



        }
}
