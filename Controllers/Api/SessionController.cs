using Bakers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bakers.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public string GetSessionInfo()
        {
            //List<string> sessionInfo = new List<string>();
            //if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(SessionVariables.SessionKeyUsername)))
            //{
            //    HttpContext.Session.SetString(SessionVariables.SessionKeyUsername, "Current Client");
            //    HttpContext.Session.SetString(SessionVariables.SessionKeySessionId, Guid.NewGuid().ToString());
            //}

            //var username = HttpContext.Session.GetString(SessionVariables.SessionKeyUsername);
            //var sessionId = HttpContext.Session.GetString(SessionVariables.SessionKeySessionId);

            //sessionInfo.Add(username);
            //sessionInfo.Add(sessionId);

            return "Hello World";
        }
    }
}
