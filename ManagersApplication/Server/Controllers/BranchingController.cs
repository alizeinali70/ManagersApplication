using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchingController : ControllerBase
    {
        [HttpGet]
        public async Task<List<Branching>> GetAsync()
        {
            OracleDBContext? context = HttpContext.RequestServices.GetService(typeof(OracleDBContext)) as OracleDBContext;
            return await context.GetAllAsync();
        }
    }
}
