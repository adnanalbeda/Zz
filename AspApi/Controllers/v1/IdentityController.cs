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
    public Task<ActionResult<Login.HResult>> Login([FromBody] Login.Request request)
    {
        return HandleAppRequest<Login.Request, Login.HResult>(request);
    }

    [HttpPost("hi")]
    public Task<ActionResult<string>> Hi([FromQuery] string name)
    {
        return Task.FromResult((ActionResult<string>)Ok(name));
    }

    [Sensitive]
    [HttpPost("secret")]
    public Task<ActionResult<string>> Secret([FromQuery] string name)
    {
        return Task.FromResult((ActionResult<string>)Ok(name));
    }
}
