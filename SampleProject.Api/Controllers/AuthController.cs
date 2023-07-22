using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleProject.Business.Handlers.Authorizations.Commands;
using SampleProject.Business.Handlers.Authorizations.Queries;
using SampleProject.Core.Utilities.Results;
using SampleProject.Entities.Concrete;
using SampleProject.Entities.Dtos;
using ServiceStack;

namespace SampleProject.Api.Controllers
{
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage<User>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginUserDto)
        {
            return CreateActionResult(await Mediator.Send(new LoginUserQuery() { LoginModel = loginUserDto }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMessage<User>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDto userDto)
        {
            return CreateActionResult(await Mediator.Send(new RegisterUserCommand() { Model = userDto }));
        }
    }
}
