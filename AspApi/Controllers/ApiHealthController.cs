namespace Zz.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zz.DataBase;

[AllowAnonymous]
public class ApiHealthController : BaseApiVersionedController
{
    [HttpGet("is-healthy")]
    public async Task<ActionResult> IsHealthy([FromServices] ILogger<ApiHealthController> logger)
    {
        bool faulty = false;
        // Test Services that must be alive and working.
        try
        {
            if (
                !await HttpContext
                    .RequestServices.GetRequiredService<DataContext>()
                    .Database.CanConnectAsync()
            )
                throw new InvalidOperationException("Cannot connect to App DB.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            faulty = true;
        }

        try
        {
            if (
                !await HttpContext
                    .RequestServices.GetRequiredService<MoneyFXDataContext>()
                    .Database.CanConnectAsync()
            )
                throw new InvalidOperationException("Cannot connect to MoneyFX DB.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            faulty = true;
        }

        return faulty ? StatusCode(498) : Ok();
    }

    [HttpGet("is-available")]
    public ActionResult IsAvailable()
    {
        return Ok();
    }
}
