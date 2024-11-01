using System.Text.Json.Serialization;
using EntityFramework.Exceptions.Sqlite;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zz.DataBase;
using Zz.DataBase.Identity;
using Zz.Model.Identity;

namespace System;

public static partial class ZzDependencyInjectionExtensions
{
    public static IHostApplicationBuilder AddZzAppDbContexts(this IHostApplicationBuilder builder)
    {
        Console.WriteLine("Configure App DB Service...");

        builder
            .Services.AddDbContext<DataContext>(o =>
                o.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        o =>
                        {
                            o.EnableRetryOnFailure(1);
                            o.CommandTimeout(30);
                        }
                    )
                    .UseExceptionProcessor()
            )
            .AddDbContext<UserSessionsDataContext>(o =>
                o.UseSqlServer(
                        builder.Configuration.GetConnectionString("UserSessions_DBConnection"),
                        o =>
                        {
                            o.EnableRetryOnFailure(1);
                            o.CommandTimeout(10);
                        }
                    )
                    .UseExceptionProcessor()
            )
            .AddDbContext<MoneyFXDataContext>(o =>
                o.UseSqlServer(
                        builder.Configuration.GetConnectionString("MoneyFX_DBConnection"),
                        o =>
                        {
                            o.EnableRetryOnFailure(1);
                            o.CommandTimeout(10);
                        }
                    )
                    .UseExceptionProcessor()
            );

        return builder;
    }
}
