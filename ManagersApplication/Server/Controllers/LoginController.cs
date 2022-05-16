using ManagersApplication.Server.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApplication.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }
        /// <summary>

        /// </summary>
        [HttpGet("[controller]/[action]")]
        [Route("Login/Login_Async")]
        public async Task<string> Login_Async([FromBody] string username)
        {
            try
            {
                DBContext context = new DBContext(_config);
                var uname = await context.LoginAsync(username);

                return uname;
            }
            catch (Exception exp)
            {
                throw;
            }
        }


    }
}
