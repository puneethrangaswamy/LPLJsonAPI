using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TopNavApplication.Helper;
using TopNavApplication.Model;
using TopNavApplication.Model.response;

namespace TopNavApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LPLMenuAPIController : ControllerBase
    {

        [HttpPost]
        public String Auth(Login login)
        {
            Console.WriteLine("Username => " + login.username);
            Console.WriteLine("Password => " + login.password);

            if (String.IsNullOrEmpty(login.username) || String.IsNullOrEmpty(login.password))
            {

            }

            return "Login Successfull!!";
        }

        [HttpGet ("apps/")]
        public async Task<ActionResult> GetAllApplications()
        {
            return Ok(LPLMenuDataContext.getApplication("av"));
        }

    }

    
}
