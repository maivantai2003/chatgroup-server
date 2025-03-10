using chatgroup_server.Interfaces.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Text;

namespace chatgroup_server.Extensions
{
    public static class AuthExtention
    {
        public static void AddAuthenConfiguration(this IServiceCollection services,IConfiguration _configuration)
        {
            var jwtKey = _configuration.GetValue<string>("Jwt:key");
            var keyBytes=Encoding.ASCII.GetBytes(jwtKey);
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero,
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters= tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    //OnMessageReceived = context =>
                    //{
                    //    var accessToken = context.Request.Cookies["accessToken"];
                    //    if (!string.IsNullOrEmpty(accessToken))
                    //    {
                    //        context.Token = accessToken;
                    //    }
                    //    return Task.CompletedTask;
                    //},
                    OnTokenValidated = async (context) =>
                    {
                        var ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
                        var jwtService = context.Request.HttpContext.RequestServices.GetService<IJwtService>();
                        var jwtToken = context.SecurityToken as JwtSecurityToken;

                        //if (jwtService != null && jwtToken != null)
                        //{
                        //    bool isValid = await jwtService.IsTokenValid(jwtToken.RawData, ipAddress);
                        //    if (!isValid)
                        //    {
                        //        context.Fail("Invalid Token Details");
                        //    }
                        //}
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/app-hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    //OnAuthenticationFailed = context =>
                    //{
                    //    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    //    return Task.CompletedTask;
                    //}
                };
            });
            services.AddHttpContextAccessor();
        }

    }
}
