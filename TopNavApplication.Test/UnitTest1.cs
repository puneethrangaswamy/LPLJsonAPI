using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TopNavApplication.ApiControllers;
using TopNavApplication.Model.response;
using TopNavApplication.Util;

namespace TopNavApplication.Test
{
    public class UnitTest1
    {
        

        [Fact]
        public void VerifyJWT()
        {
            Assert.False(TokenUtil.ValidateToken("auth"));
        }

        [Fact]
        public void testController()
        {
            Mock<HttpContext> moqContext = new Mock<HttpContext>();
            Mock<HttpRequest> moqRequest = new Mock<HttpRequest>();

            LPLMenuAPIController controller = new LPLMenuAPIController();
            Login login = new Login();
            login.Username = "user";
            login.Password = "password";
            Task<IActionResult> actionResult = controller.Authenticate(login);
            Assert.NotNull(actionResult);

            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);
            ActionContext actionContext = new ActionContext();
            //  controller.ControllerContext = new ControllerContext(moqContext.Object, new RouteData(), controller);


            moqContext.Setup(x => x.Request).Returns(moqRequest.Object);


            //  actionResult.
        }
    }
}