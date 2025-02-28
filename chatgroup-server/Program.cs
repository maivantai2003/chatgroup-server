using chatgroup_server.Data;
using chatgroup_server.Hubs;
using chatgroup_server.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
ConfigurationManager configuration = builder.Configuration;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")); });
builder.Services.AddApplication();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["RedisCacheUrl"];
});
builder.Services.AddAuthenConfiguration(configuration);
//builder.Services.AddSwaggerGen(opt =>
//{
//    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "JWTRefreshTokens", Version = "v1" });
//    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });
//    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type=ReferenceType.SecurityScheme,
//                    Id="Bearer"
//                },
//                Scheme="oauth2",
//                Name="Bearer",
//                In= ParameterLocation.Header,

//            },
//            new string[]{}
//        }
//    });
//});
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("Logs/app-log.txt",
//    rollingInterval: RollingInterval.Day, shared: true).CreateLogger();
//builder.Host.UseSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());
app.MapHub<myHub>("/app-hub");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
