using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagersApplication.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BranchingController : ControllerBase
    {       

        [HttpGet]
        public async Task<List<Branching>> GetAsync()
        {
            BranchingDBContext? context = HttpContext.RequestServices.GetService(typeof(BranchingDBContext)) as BranchingDBContext;
            return await context.GetAllAsync();

        }
    }
}
