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
        public readonly IConfiguration _config;

        public BranchingController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<List<Branching>> GetAsync()
        {
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                List<Branching> list = await context.GetAllAsync();
                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
    }
}
