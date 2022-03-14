using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagersApplication.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[function]")]
    [ApiController]
    public class BranchingController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BranchingController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]        
        public async Task<ActionResult<List<Branching>>> Get_All_Contract_Async()
        {
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                List<Branching> list = await context.GetAllContractAsync();
                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        [HttpGet]        
        public async Task<ActionResult<Branching_Item>> Get_Requester_Name_Async(string RQID)
        {
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                Branching_Item _Item = await context.GetNameAsync(RQID);
                return _Item;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
    }
}
