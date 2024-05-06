using CircuitsUc.Api.Controllers;
using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SecurityUserDTO;
using CircuitsUc.Application.IServices;
using CircuitsUc.Application.Services;
using CircuitsUc.Domain.Entities;
using CircuitsUc.Domain.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalBlogController : BaseApiController
    {
   
        private readonly ISecurityUserService _SecurityUser;
        public PersonalBlogController(ISecurityUserService SecurityUser)
        {
           
            _SecurityUser = SecurityUser;
        }
        [HttpGet("GetAll")]
        public async Task<GeneralResponse<List<SecurityUserDto>>> GetAll()
        {
           
            return await _SecurityUser.GetAll();
        }
        [HttpGet("GetById")]
        public async Task<GeneralResponse<SecurityUserDto>> GetById(Guid Id)
        {
           return await _SecurityUser.GetByIdAsync(Id);
        }

       
      
        [HttpPost("Add")]
        public async Task<GeneralResponse<Guid>> Add(SecurityUserInput SecUser)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _SecurityUser.Add(SecUser, userId);
        }
      
        [HttpPut("Update")]
        public async Task<GeneralResponse<Guid>> Update(SecurityUserUpdateInput SecUser)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _SecurityUser.Update(SecUser,userId);
        }
        [HttpDelete("Delete")]
        public async Task<GeneralResponse<Guid>> Delete(Guid Id)
        {
            return await _SecurityUser.SoftDelete(Id);
        }
        [HttpPost("SoftRangeDelete")]
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> id)
        {
            return await _SecurityUser.SoftRangeDelete(id);
        }

    }
}
