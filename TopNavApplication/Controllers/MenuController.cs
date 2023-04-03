using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using TopNavApplication.Repositories;
using TopNavApplication.Services;
using IAuthenticationService = TopNavApplication.Services.IAuthenticationService;

namespace TopNavApplication.ApiControllers
{
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly IMenuService _menuService;

        public MenuController(IAuthenticationService authenticationService, 
                              ITokenService tokenService, 
                              IMenuService menuService) 
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _menuService = menuService;
        }

        [HttpPost("LPLMenuAPI/auth")]
        public async Task<IActionResult> Login([FromBody]LoginCredentials credentials)
        {
            var isAuthenticated = await _authenticationService.Authenticate(credentials);

            if (!isAuthenticated)
            {
                return BadRequest("Please provide valid credentials");
            }

            string token = await _tokenService.CreateToken(credentials);

            string? groupName = _tokenService.GetRoleName(token);

            if (string.IsNullOrWhiteSpace(groupName))
            {
                return BadRequest("Invalid groupName");
            }

            var headers = HttpContext.Response.Headers;
            headers.Add("Access-Control-Expose-Headers", "*");

            string json = SerializeAuthenticationResponse(credentials.Username, groupName!, token);

            return Ok(json);
        }

        [HttpGet("LPLMenuAPI/validateauth"), Authorize(Roles = "GLOBAL,INVESTOR,CLIENT")]
        public async Task<IActionResult> ValidateAuthenticationToken()
        {
            var token = HttpContext.Request.Headers.Authorization.Single();

            if (!_tokenService.IsValidToken(token)) // TODO: remove after enabling validation via TokenValidationParameters 
            {
                return BadRequest("Invalid token");
            }

            string? userName = _tokenService.GetUserName(token);
            if (userName == null)
            {
                return BadRequest("Invalid userName");
            }

            string groupName = await LPLMenuDataContext.GetRoleByUserName(userName);
            if (groupName == null)
            {
                return BadRequest("Invalid groupName");
            }

            var headers = HttpContext.Response.Headers;
            headers.Add("Access-Control-Expose-Headers", "*");
            headers.Add("userName", userName);
            headers.Add("x-auth-token", token);

            string json = SerializeAuthenticationResponse(userName, groupName, token);

            return Ok(json);
        }

        [HttpGet("LPLMenuAPI/postAuth"), Authorize(Roles = "GLOBAL,INVESTOR,CLIENT")]
        public async Task<ActionResult> GetMenuItems([Required] string appName, [Required] string groupName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appName))
                {
                    return BadRequest($"Please provide {nameof(appName)}");
                }

                if (string.IsNullOrWhiteSpace(groupName))
                {
                    return BadRequest($"Please provide {nameof(groupName)}");
                }

                var application = await LPLMenuDataContext.GetApplication(appName);

                if (null == application)
                {
                    return BadRequest($"No application found with name of {appName}");
                }

                var postAuthMenu = await _menuService.GetMenuItems(groupName, appName);

                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");

                string json = JsonConvert.SerializeObject(postAuthMenu);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string SerializeAuthenticationResponse(string userName, string groupName, string token)
        {
            var authResponseonse = new
            {
                userName,
                groupName,
                token
            };

            return SerializeToJson(authResponseonse);
        }

        private string SerializeToJson(object value)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(value, serializerSettings);

            return json;
        }
    }
}
