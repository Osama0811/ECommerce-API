﻿using AutoMapper;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.Helpers;
using CircuitsUc.Application.IService;
using CircuitsUc.Application.Models.AuthDTO;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using CircuitsUc.Domain.IRepositories.Base;
using CircuitsUc.Application.Common.SharedResources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CircuitsUc.Application.Helpers.CommenEnum;
using CircuitsUc.Application.IServices;

namespace CircuitsUc.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unit;
        private readonly IStringLocalizer<GeneralMessages> _localization;
        private readonly IConfiguration _config;
        private readonly ISecurityUserService _userService;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unit, IStringLocalizer<GeneralMessages> localization, IConfiguration config, IMapper Mapper
            ,ISecurityUserService userService,IDocumentService documentService)
        {
            _unit = unit;
            _localization = localization;
            _config = config;
            _mapper = Mapper;
            _userService = userService;
            _documentService = documentService;
        }

       

        public async Task<GeneralResponse<AuthResponse>> Login(AuthRequest request)
        {
            #region CheckIsExist
            var Pass = WebUiUtility.Encrypt(request.Password);
            var User =  _unit.SecurityUser.All().Where(x => x.Email == request.Email && x.Password ==Pass).FirstOrDefault();
            if (User == null)
            {
               
                    return new GeneralResponse<AuthResponse>(_localization["UserNotFound"], System.Net.HttpStatusCode.BadRequest);
            }
            User.LastLoginDate = DateTime.Now;
            User.IsOnline = true;
            await _unit.SecurityUser.UpdateAsync(User);
            await _unit.SaveAsync();
            #endregion
            string ImagePath = "", FileName = "";
            var _enumVal = User.RoleId.ToString();
            Document currentDoc =  _documentService.GetMainDocumentByEntity(User.Id.ToString(), _enumVal.ToString());
            if (currentDoc != null)
            {
                ImagePath = _documentService.GetImagePath
                    (User.Id, currentDoc.Id,
                    _enumVal, currentDoc.FileName);

                FileName = currentDoc.FileName;
            }

            #region Generate JWT
            JwtSecurityToken jwtSecurityToken = await GenerateToken(User, DateTime.UtcNow.AddYears(3));
            AuthResponse response = new AuthResponse
            {
                UserId = User.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = User.Email,
                UserName = User.UserName,
                ImagePath = ImagePath,
                FileName = FileName,
               
                RoleId = User.RoleId
            };

            #endregion
            return new GeneralResponse<AuthResponse>(response, _localization["LoginSuccessfully"].Value, 0);
            
        }

       
       
       
       
        public async Task<GeneralResponse<ChangeUserPasswordResponse>> ChangePassword(ChangeUserPasswordRequest request)
        {

            var SecurityUser = _unit.SecurityUser.All().Where(x => x.Id == request.Id).FirstOrDefault();
            var oldpassword = WebUiUtility.Encrypt(request.CurrentPassword);
            if (oldpassword != SecurityUser.Password)
            {
                return new GeneralResponse<ChangeUserPasswordResponse>(_localization["PassIsNotCorrect"], System.Net.HttpStatusCode.BadRequest);
            }
            SecurityUser.Password = WebUiUtility.Encrypt(request.NewPassword);

            await _unit.SecurityUser.UpdateAsync(SecurityUser);
            var results = _unit.Save();
            JwtSecurityToken jwtSecurityToken = await GenerateToken(SecurityUser, DateTime.UtcNow.AddMinutes(5));
            ChangeUserPasswordResponse response = new ChangeUserPasswordResponse
            {
                Id = SecurityUser.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),


            };
            return results >= 1 ?
           new GeneralResponse<ChangeUserPasswordResponse>(response, _localization["ChangePasswordSuccessfully"], 0)
           : new GeneralResponse<ChangeUserPasswordResponse>(_localization["ErrorInChangePassword"], System.Net.HttpStatusCode.BadRequest);

        }

        public async Task<bool> Logout(Guid UserID)
        {

            var User = _unit.SecurityUser.All().Where(z => z.Id == UserID).FirstOrDefault();
            User.IsOnline = false;
            var Result = await _unit.SaveAsync();
            if (Result > 0)
            {
                return true;
            }
            return false;
        }
        private async Task<JwtSecurityToken> GenerateToken(SecurityUser user, DateTime expiredDate)
        {

            var claims = new[]
               {
                        new Claim("user_id", user.Id.ToString()),
                        new Claim("role_id", ((RoleType)user.RoleId).ToString())
                 };

            

            var _jwtSettings = new JwtSettings(_config);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
          
            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
                claims: claims,
                expires:   expiredDate,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature));
            return jwtSecurityToken;
        }
        public string ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var _jwtSettings = new JwtSettings(_config);
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = _jwtSettings.ValidateLifeTime,
            };
            var principal = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
            if (principal == null)
                throw new SecurityTokenException("Invalid token");
            try
            {
                return ((ClaimsIdentity)principal.Identity).FindFirst("user_id").Value;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            return null;
        }


    }
}