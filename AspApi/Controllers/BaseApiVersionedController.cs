namespace Zz.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class BaseApiVersionedController : BaseApiController { }
