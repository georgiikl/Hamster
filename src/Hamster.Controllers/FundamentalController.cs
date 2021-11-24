using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hamster.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class FundamentalController : ControllerBase
    {
        [HttpGet]
        public Task<string> Resources()
        {
            return Task.FromResult("Hello!");
        }
    }
}