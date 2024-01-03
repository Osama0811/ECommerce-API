using CircuitsUc.Api.Controllers;
using CircuitsUc.Api.Extentions;
using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.SystemParameterDTO;
using CircuitsUc.Application.IServices;
using CircuitsUc.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CircuitsUc.Application.Helpers.CommenEnum;

namespace CircuitsUc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemParameterController : BaseApiController
    {
        private readonly ISystemParameterServices _SystemParameter;
        public SystemParameterController(ISystemParameterServices SystemParameter)
        {

            _SystemParameter = SystemParameter;
        }
        [HttpGet("GetAll")]
        public async Task<GeneralResponse<List<SystemParameterDto>>> GetAll()
        {
            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _SystemParameter.GetAll(isEnglish);
        }
        [HttpGet("GetById")]
        public async Task<GeneralResponse<SystemParameterDto>> GetById(Guid Id)
        {
            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");

            return await _SystemParameter.GetByIdAsync(Id,isEnglish);
        }

        [HttpGet("GetAllASync")]
        public async Task<GeneralResponse<List<SystemParameter>>> GetAllAsync()
        {
            return await _SystemParameter.GetAllAsync();
        }
      
        [HttpPost("Add")]
        public async Task<GeneralResponse<Guid>> Add(SystemParameterInput SystemParameter)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _SystemParameter.Add(SystemParameter,userId);
        }
      
        [HttpPost("Update")]
        public async Task<GeneralResponse<Guid>> Update(SystemParameterUpdateInput systemParameter)
        {
            Guid userId = Guid.Parse(HttpContext.GetUserId());
            return await _SystemParameter.Update(systemParameter,userId);
        }
        [HttpPost("Delete")]
        public async Task<GeneralResponse<Guid>> Delete(Guid id)
        {
            return await _SystemParameter.DeleteByIdAsync(id);
        }
        [HttpPost("DeleteImage")]
        public async Task<GeneralResponse<Guid>> DeleteImage(Guid Id)
        {
            return await _SystemParameter.DeleteImage(Id);
        }
        [HttpPost("SoftDelete")]
        public async Task<GeneralResponse<Guid>> SoftDelete(Guid id)
        {
            return await _SystemParameter.SoftDelete(id);
        }
        [HttpPost("SoftRangeDelete")]
        public async Task<GeneralResponse<List<Guid>>> SoftRangeDelete(List<Guid> id)
        {
            return await _SystemParameter.SoftRangeDelete(id);
        }
        [HttpGet("InviteApp")]
        public async Task<GeneralResponse<Object>> InviteApp()
        {
            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return await _SystemParameter.GetSystemParameterByKeyName(SystemParameterKey.InviteApp.ToString(),isEnglish);
        }
      
    }
}
