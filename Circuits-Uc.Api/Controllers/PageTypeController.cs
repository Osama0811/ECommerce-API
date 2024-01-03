using CircuitsUc.Application.Communications;
using CircuitsUc.Application.DTOS.PageTypeDTO;
using CircuitsUc.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageTypeController : BaseApiController
    {

        private readonly IPageTypeService _PageType;
        public PageTypeController(IPageTypeService PageType)
        {
            _PageType = PageType;
        }
        [HttpGet("PageTypeDDL")]
        public GeneralResponse<List<PageTypeDropDown>> PageTypeDDL()
        {
            bool isEnglish = Request.Headers["Accept-Language"].ToString().ToLower().Contains("en");
            return _PageType.PageTypeDDL(isEnglish);
        }
    }
}
