using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagersApplication.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    public class BranchingController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BranchingController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("[controller]/[action]")]
        [Route("Branching/Get_All_Contract_Async")]
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

        [HttpGet("[controller]/[action]")]
        [Route("Branching/Get_Requester_Name_Async/")]
        public async Task<ActionResult<Branching_Item>> Get_Requester_Name_Async([FromBody]string RQID)
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

        [HttpPost("[controller]/[action]")]
        [Route("Branching/Get_RequestRow_Async/")]
        public async Task<ActionResult<List<Branching_Item>>> Get_RequestRow_Async([FromBody] string RQID)
        {
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                List<Branching_Item> list = await context.GetRquestRowAsync(RQID);
                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
    }
}
