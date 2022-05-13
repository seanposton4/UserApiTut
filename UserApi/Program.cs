// Imports
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using UserApi.Context;
using UserApi.Contracts;
using UserApi.Filters;

using System.IdentityModel.Tokens.Jwt;
using UserApi.Repository;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

// Services
var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                            builder =>
                            {
                                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                            });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtKey:secret").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
      };
    }
);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();