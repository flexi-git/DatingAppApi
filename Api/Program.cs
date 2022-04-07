using Api.Data;
using Api.Extensions;
using Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddScoped<ITokenService, TokenService>();

// Add MyDbContext to Dependency Injection
builder.Services.AddDbContext<ApiDataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient); //// this will reset your model to its original value if you decided to cancel the operations.

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddIdentityServices(configuration);


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:SecretKey"])),
//            ValidateIssuer = false,
//            ValidateAudience = false,
//        };
//    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
