
using Azure.Core;
using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Common.SharedResources;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.IServices;
using CircuitsUc.Application.Models.AuthDTO;
using CircuitsUc.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ISecurityUserService _UserService;
        private readonly IStringLocalizer<GeneralMessages> _localization;
        public AuthenticationController(IAuthService authenticationService, IStringLocalizer<GeneralMessages> localization, ISecurityUserService userService)
        {
            _authenticationService = authenticationService;
            _localization = localization;
            _UserService = userService;
        }

        [HttpPost("login")]
        public async Task<GeneralResponse<AuthResponse>> Login(AuthRequest request)
        {

            //285180
            var password = WebUiUtility.Decrypt("bRfSm3N1FM9ZSj3VebIS8A,,");
            return await _authenticationService.Login(request);
        }


        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<GeneralResponse<ChangeUserPasswordResponse>> ChangePassword(ChangeUserPasswordRequest request)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());

            return await _authenticationService.ChangePassword(request, userId);
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



        [HttpPost("SetAdmin")]
        public async Task<GeneralResponse<SecurityUser>> SetAdmin()
        {
            return await _UserService.SetAdmin();
        }

        }
}
