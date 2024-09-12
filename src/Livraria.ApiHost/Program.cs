#region [ -- Usings -- ]

using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Livraria.ApiHost.Autofac;
using Livraria.ApiHost.Configuration;
using Livraria.Core.Infrastructure;
using Livraria.Infrastructure;
using FluentValidation;
using Livraria.Domain.Commands.Books;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region [ -- Serilog -- ]

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Application starting up in {EnvironmentName} mode", builder.Environment.EnvironmentName);

builder.Logging.ClearProviders();


builder.Host.UseSerilog((ctx, serviceProvider, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.WithProperty("ApplicationName", "Backend"));

#endregion

#region [ -- Hybrid Cache -- ]

builder.Services.AddHybridCache();

#endregion

#region [ -- Autofac -- ]

builder.Services.AddHttpContextAccessor();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    b.RegisterModule<CoreModule>();

    // Unit Of Work
    b.RegisterType<UnitOfWork<LivrariaDbContext>>()
        .As<IUnitOfWork<LivrariaDbContext>>()
        .InstancePerLifetimeScope();

    b.RegisterType<UnitOfWork<LivrariaDbContext>>()
        .As<IUnitOfWork<LivrariaDbContext>>()
        .InstancePerLifetimeScope();
});

#endregion

#region [ -- Entity Framework -- ]

builder.Services.AddDbContext<LivrariaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreDatabase"));
});

#endregion

#region [ -- Controllers -- ]

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

builder.Services.AddControllers();

#endregion

#region [ -- Healthcheck -- ]

builder.Services.AddHealthChecks();

#endregion

#region [ -- Fluent Validation -- ]

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

#endregion

#region [ -- Swagger -- ]

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    
    c.DocumentFilter<SwaggerAddEnumDescriptions>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddFluentValidationRulesToSwagger();

#endregion

#region [ -- CORS -- ]

builder.Services.AddCors(o =>
{
    o.AddPolicy("Development", c =>
    {
        c
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true);
    });

    o.AddPolicy("Staging", c =>
    {
        c
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true);
    });


    o.AddPolicy("Production", c =>
    {
        c
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(hostName => true);
    });
});

#endregion

var app = builder.Build();

app.UseSerilogRequestLogging();

var dbContext = app.Services.GetRequiredService<LivrariaDbContext>();
await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region [ -- USE CORS -- ]

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}

if (app.Environment.IsStaging())
{
    app.UseCors("Staging");
}
else if (app.Environment.IsProduction())
{
    app.UseCors("Production");
}

#endregion

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();


namespace Livraria.ApiHost
{
    public partial class Program;
}