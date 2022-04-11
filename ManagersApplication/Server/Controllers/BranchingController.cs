using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                foreach (var item in list)
                {
                    item.UPDT_DATE = ConvertDate.MiladiToShamsi(item.UPDT_DATE);

                    // item.UPDT_DATE = Convert.ToDateTime(ConvertDate.MiladiToShamsi(item.UPDT_DATE.ToString("yyyy/MM/dd"))).Date;                   
                }
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
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                Contract_Item contract_Item = await context.GetAdmContract(RQID);

                contract_Item.Cont_Date = ConvertDate.MiladiToShamsi(contract_Item.Cont_Date);

                contract_Item.View_Date = ConvertDate.MiladiToShamsi(contract_Item.View_Date);


                return contract_Item;
            }
            catch (Exception exp)
            {

                throw;
            }


        }

        [HttpPost("[controller]/[action]")]
        [Route("Branching/ConfirmContractasync/")]
        public async Task<ActionResult<string>> ConfirmContractasync([FromBody] string RQID)
        {
            try
            {
                SelectDBContext context = new SelectDBContext(_config);
                string res = await context.ConfirmContract(RQID);
                return res;
            }
            catch (Exception exp)
            {

                throw;
            }
}
    }
}
