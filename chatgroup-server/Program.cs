using chatgroup_server.Data;
using chatgroup_server.Extensions;
using chatgroup_server.Helpers;
using chatgroup_server.Hubs;
using chatgroup_server.Middlewares;
using chatgroup_server.RabbitMQ.Consumer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text.Json;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();
ConfigurationManager configurations = builder.Configuration;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.AddApplication();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configurations["REDIS_HOST"];
});

builder.Services.AddAuthenConfiguration(configurations);
builder.Services.AddHealthChecks()
    .AddRedis(configurations["REDIS_HOST"], name: "redis", tags: new[] { "ready", "redis" })
    .AddSqlServer(configurations.GetConnectionString("DefaultConnection"), name: "sqlserver", tags: new[] { "ready", "sqlserver" });
var healthCheckHost = Environment.GetEnvironmentVariable("HEALTHCHECK_HOST") ?? "localhost";
builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(30);
    //options.AddHealthCheckEndpoint("ChapApp Health", "/health");
    options.AddHealthCheckEndpoint("ChapApp Health", $"http://{healthCheckHost}:80/health");
}).AddInMemoryStorage();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ChapApp-Server", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In= ParameterLocation.Header,

            },
            new string[]{}
        }
    });
});
configureLogging();
builder.Host.UseSerilog();
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var userId = context.User?.FindFirst("sub")?.Value
                     ?? context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var partitionKey = !string.IsNullOrEmpty(userId)
            ? $"user:{userId}"
            : $"ip:{context.Connection.RemoteIpAddress?.ToString()}";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: partitionKey,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60, // 20 request
                Window = TimeSpan.FromSeconds(60),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 20
            });
    });
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var userId = context.HttpContext.User?.FindFirst("sub")?.Value
                     ?? context.HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

        // Logging
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogWarning("[RateLimit] User: {UserId} IP: {IP}", userId ?? "guest", ip);
        Console.WriteLine($"[RateLimit] User: {userId ?? "guest"} IP: {ip}");

        var result = new
        {
            error = "Bạn đã gửi quá nhiều request, vui lòng thử lại sau."
        };

        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(result), cancellationToken);
    };
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
var swaggerEnabled = Environment.GetEnvironmentVariable("ENABLE_SWAGGER");

if (app.Environment.IsDevelopment() || swaggerEnabled == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});
app.MapHub<myHub>("/app-hub");
app.MapControllers();
app.Run();
void configureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    //var configuration = new ConfigurationBuilder().AddEnvironmentVariables().AddJsonFile("appsettings.json",optional:true,reloadOnChange:true)
    //    .AddJsonFile($"appsettings.{environment}.json",optional:true).Build();
    //Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().Enrich.WithExceptionDetails()
    //    .WriteTo.Debug().WriteTo.Console().WriteTo.Elasticsearch(ConfigurationElasticSink(configuration,environment))
    //    .Enrich.WithProperty("Environment",environment).ReadFrom.Configuration(configuration)
    //    .CreateLogger();
    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables() // <- Quan trọng
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigurationElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}
ElasticsearchSinkOptions ConfigurationElasticSink(IConfigurationRoot configuration,string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat=$"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".","-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfReplicas=1,
        NumberOfShards=2      
    };
}