using System.Diagnostics;
using System.Security.Claims;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zz;
using Zz.DataBase;
using Zz.DataBase.Identity;
using Zz.Model.Identity;

//========================//
//=== CREATE HOST BUILDER
//========================//

Console.WriteLine("Creating Web App Builder...");
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

//===============================//
//=== CONFIGURE HOSTING SERVICES
//===============================//

Console.WriteLine("Configure Kestrel...");

builder.WebHost.ConfigureKestrel(opt =>
{
    //! Security: Remove Server Type From Header
    opt.AddServerHeader = false;
    //! Performance: To prevent request remain for a long time.
    opt.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(90);
});

builder.ConfigureZzCors().ConfigureZzJwtTokens().ConfigureZzMvcSignalR().ConfigureZzVersioning();

#if DEBUG
builder.ConfigureZzSwagger().ConfigureZzDebugLogging();
#endif

builder
    .AddZzAppDbContexts()
    .AddZzAppIdentity()
    .Services.AddHttpContextAccessor()
    .AddZzAppConfigs()
    .AddZzAppServices();

# region App

Console.WriteLine("Building App...");

try
{
    var app = builder.Build();

    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    try
    {
        await ApplyDBMigrationAndSeed(app, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(
            ex,
            "Preparing app to start has failed. Couldn't run app db migrations: {EX_MSG}",
            ex.Message
        );
        return;
    }

    logger.LogInformation(
        "App services & configurations are built successfully. Setup Middlewares..."
    );

#if DEBUG
    //============ Serve Swagger

    logger.LogDebug("1- Swagger...");

    // Configure the HTTP request pipeline.
    app.UseZzSwagger();
#endif

    logger.LogDebug("2- Security Headers...");

    // None of them matters if hosting behind NGINX.
    // Let NGINX handle them instead.
    app.UseZzProxySecurityHeaders();

    logger.LogDebug("3- Static Files...");

    app.UseDefaultFiles();
    app.UseStaticFiles();

    logger.LogDebug("_- Allow context buffering for logging...");
    app.Use(
        (context, next) =>
        {
            context.Request.EnableBuffering();
            return next();
        }
    );

    logger.LogDebug("4- Allow context buffering for logging...");
    app.UseZzExceptionMiddleware();

    logger.LogDebug("5- Browse Routes...");
    app.UseRouting();

    //============ Authentication
    // An exception says UseAuthentication must be between routing and mapping.

    logger.LogDebug("6- Authenticate/Authorize...");

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseZzSessionValidatorMiddleware();

#if DEBUG
    if (app.Environment.IsDevelopment())
    {
        logger.LogDebug("_- Monkey Middleware (for testing purposes)...");
        app.UseZzMonkeyMiddleware();

        logger.LogDebug("_- HttpLogging Middleware (for testing purposes)...");
        app.UseHttpLogging();
    }
#endif

    logger.LogDebug("7- Map Route to Controller/Hub...");

    app.MapControllers();

    logger.LogDebug("8- Fallback to index.html...");
    app.MapFallbackToFile("/index.html");

    logger.LogDebug("=> Running Api...");
    try
    {
        await app.RunAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "App failed to start: {EX_MSG}", ex.Message);
    }
}
catch (Exception ex)
{
    await Console.Error.WriteLineAsync("App Failed to start: ");

    await Console.Error.WriteLineAsync(ex.Message);
    await Console.Error.WriteLineAsync(ex.InnerException?.Message ?? "-");
    await Console.Error.WriteLineAsync(ex.InnerException?.InnerException?.Message ?? "--");
    await Console.Error.WriteLineAsync(ex.StackTrace);

#if DEBUG
    Trace.TraceError(ex.Message);
#endif
}

async Task ApplyDBMigrationAndSeed(Microsoft.AspNetCore.Builder.WebApplication app, ILogger logger)
{
    logger.LogDebug("Apply Recent Migrations...");

    using var scope = app.Services.CreateAsyncScope();

    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    var moneyFXDbContext = scope.ServiceProvider.GetRequiredService<MoneyFXDataContext>();
    var userSessionDbContext = scope.ServiceProvider.GetRequiredService<UserSessionsDataContext>();

    await dataContext.Database.MigrateAsync();
    await moneyFXDbContext.Database.MigrateAsync();
    await userSessionDbContext.Database.MigrateAsync();

    using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    using var userManger = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var id = Id22.New();

    if (!await userManger.Users.AnyAsync())
    {
        var res = await userManger.CreateAsync(
            new User(id, "Zz@admin")
            {
                Profile = new(id),
                MetaData = new(id),
                Settings = new(id),
            },
            "Pa$$w0rd"
        );
        if (res.Succeeded)
            return;

        foreach (var err in res.Errors)
            Console.WriteLine(err.Code);

        throw new InvalidOperationException("Error occurred.");
    }
}


#endregion
