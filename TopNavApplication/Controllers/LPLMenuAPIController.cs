using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopNavApplication.Model.response;

namespace TopNavApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LPLMenuAPIController : Controller
    {

        [HttpPost]
        public ActionResult Auth(Login login)
        {
            Console.WriteLine("Username => " + login.username);
            Console.WriteLine("Password => " + login.password);

            if (String.IsNullOrEmpty(login.username) || String.IsNullOrEmpty(login.password))
            {

            }

            return View();      
        }

    }
}
