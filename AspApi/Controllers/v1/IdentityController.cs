#if DEBUG
namespace Zz.Api.Controllers;

using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zz.App.Requests;

[ApiVersion("1.0")]
public class IdentityController : BaseApiVersionedController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public Task<ActionResult<Login.HResult>> Req([FromBody] Login.Request request)
    {
        return HandleAppRequest<Login.Request, Login.HResult>(request);
    }

    [HttpPost("hi")]
    public Task<ActionResult<string>> Req([FromQuery] string name)
    {
        return Task.FromResult((ActionResult<string>)Ok(name));
    }
}
#endif
