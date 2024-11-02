namespace Zz.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseApiVersionedController : BaseApiController { }
