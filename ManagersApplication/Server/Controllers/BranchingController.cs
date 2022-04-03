using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersianDate.Standard;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<ActionResult<Branching_Item>> Get_Requester_Name_Async([FromBody] string RQID)
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
                //List<Branching_Item> list = new List<Branching_Item>();
                //list.Add(new Branching_Item
                //{
                //    RQID = "123",
                //    Requster_Name = "ali",
                //    Gnrt = "test",
                //    Ampr = 200
                //});


                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        [HttpPost("[controller]/[action]")]
        [Route("Branching/Get_AdmContract/")]
        public async Task<ActionResult<Contract_Item>> Get_AdmContract([FromBody] string RQID)
        {
            PersianCalendar pc = new PersianCalendar();
            
            SelectDBContext context = new SelectDBContext(_config);
            Contract_Item contract_Item = await context.GetAdmContract(RQID);




            contract_Item.Cont_Date = ConvertDate.ToFa(contract_Item.Cont_Date).ToString();
            contract_Item.View_Date = ConvertDate.ToFa(contract_Item.View_Date).ToString();

            
            return contract_Item;
        }

    }
}
