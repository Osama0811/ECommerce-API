using CircuitsUc.Api.ActionFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CircuitsUc.Api.Controllers
{
    [ServiceFilter(typeof(CustomerBaseFilter))]
    public class CustomerBaseController : ControllerBase
    {

    }
}
