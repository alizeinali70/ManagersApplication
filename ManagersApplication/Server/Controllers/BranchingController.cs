using ManagersApplication.Server.DataAccess;
using ManagersApplication.Shared;
using Microsoft.AspNetCore.Mvc;

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
        #region Contract
        [HttpGet("[controller]/[action]")]
        [Route("Branching/Get_All_Contract_Async")]
        public async Task<ActionResult<List<Branching>>> Get_All_Contract_Async([FromBody] string username)
        {
            try
            {
                DBContext context = new DBContext(_config);
                List<Branching> list = await context.GetAllContractAsync(username);
                foreach (var item in list)
                {
                    var shamsi = ConvertDate.MiladiToShamsi(item.UPDT_DATE);
                    item.UPDT_DATE = shamsi;

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
                DBContext context = new DBContext(_config);
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
                DBContext context = new DBContext(_config);
                List<Branching_Item> list = await context.GetRquestRowAsync(RQID);
                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        [HttpPost("[controller]/[action]")]
        [Route("Branching/Get_Reject_Reason_Async/")]
        public async Task<ActionResult<List<string>>> Get_Reject_Reason_Async([FromBody] string RQID)
        {
            try
            {
                DBContext context = new DBContext(_config);
                List<string> list = await context.GetRejectReason(RQID);

                var a = context.GetRejectReason(RQID);
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
                DBContext context = new DBContext(_config);
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

        [HttpGet("[controller]/[action]")]
        [Route("Branching/Count_All_Contract_Async")]
        public async Task<ActionResult<int>> Count_All_Contract_Async([FromBody] string username)
        {
            try
            {
                DBContext context = new DBContext(_config);
                int count = await context.CountAllContractAsync(username);

                return count;
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        [HttpPost("[controller]/[action]")]
        [Route("Branching/ConfirmContractasync/")]
        public async Task<ActionResult<int>> ConfirmContractasync([FromBody] string RQID)
        {
            try
            {
                DBContext context = new DBContext(_config);
                int res = await context.ConfirmContract(RQID);
                return res;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        [HttpPost("[controller]/[action]")]
        [Route("Branching/RejectContractasync/")]
        public async Task<ActionResult<int>> RejectContractasync([FromBody] List<string> lst)
        {
            try
            {
                string RQID = lst[0];
                string Desc = lst[1];
                DBContext context = new DBContext(_config);
                int res = await context.RejectContract(RQID, Desc);
                return res;
            }
            catch (Exception exp)
            {

                throw;
            }
        }
        [HttpPost("[controller]/[action]")]
        [Route("Branching/View_Imgasync/")]
        public async Task<ActionResult<object>> View_Imgasync([FromBody] string RQID)
        {
            try
            {
                DBContext context = new DBContext(_config);
                List<object> res = await context.View_Img(RQID);
                return res;
            }
            catch (Exception exp)
            {

                throw;
            }
        }
        #endregion

        #region Confirm PriceAnnounce
        [HttpGet("[controller]/[action]")]
        [Route("Branching/Get_All_PriceAnnounce_Async")]
        public async Task<ActionResult<List<Branching>>> Get_All_PriceAnnounce_Async([FromBody] string username)
        {
            try
            {
                DBContext context = new DBContext(_config);
                List<Branching> list = await context.GetAllPriceAnnounceAsync(username);

                return list;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
        [HttpGet("[controller]/[action]")]
        [Route("Branching/Count_All_PriceAnnounce_Async")]
        public async Task<ActionResult<int>> Count_All_PriceAnnounce_Async([FromBody] string username)
        {
            try
            {
                DBContext context = new DBContext(_config);
                int count = await context.CountAllPriceAnnounceAsync(username);

                return count;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
        #endregion
    }
}
