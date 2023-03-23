using Microsoft.AspNetCore.Mvc;
using TopNavApplication.Model.response;
using TopNavApplication.Util;

namespace TopNavApplication.ApiControllers
{
    [ApiController]
    [Route("[controller]")]
    public class LPLMenuAPIController : ControllerBase
    {

        [HttpPost(Name="post")]
        public string Auth([FromBody]Login login)
        {
            Console.WriteLine("Username => " + login.username);
            Console.WriteLine("Password => " + login.password);

            if (String.IsNullOrEmpty(login.username) || String.IsNullOrEmpty(login.password))
            {
              // return BadRequest("Please provide valid credentials");
              Console.WriteLine("Please provide valid credentials");
                return "Please provide valid credentials";
            }

            if (!login.password.Equals("password")) 
            {
                //   return BadRequest("Please provide valid credentials");
                Console.WriteLine("Please provide valid credentials");
                return "Please provide valid credentials";

            }

            string token = TokenUtil.createToken(login);

            Console.WriteLine(token);

            //    response.addHeader("x-auth-token", token);
            

            //    return ResponseEntity.ok("Login Success!!!");

            //  return  CreateResponse(HttpStatusCode.OK, "Item Updated Successfully");


             return token;     
        }


        [HttpGet]
        public string ValidateAuth(string authToken)
        {
            if (String.IsNullOrEmpty(authToken)){
                return "Please provide valid token";
            }

            if (!TokenUtil.validateToken(authToken))
                return "Auth Token Expires";


            string userName = TokenUtil.getUserNameFromToken(authToken);

            //    response.addHeader("x-auth-token", authToken);

            //    return ResponseEntity.ok("Welcome " + userName);

               return userName;

        }

    }
}
