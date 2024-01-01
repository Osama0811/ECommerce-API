using CircuitsUc.Api.ActionFilter;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [ServiceFilter(typeof(BaseActionFilter))]
    public class BaseApiController : ControllerBase
    {
    }
}
