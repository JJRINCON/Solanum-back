using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Solanum_back.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SolanumContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("database") ?? throw new InvalidOperationException("Connection string 'IDMContext' not found."),
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql")));

builder.Services.AddControllers().AddJsonOptions( options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddCors(options => options.AddPolicy("AllowWebApp",
                         builder => builder.AllowAnyOrigin()
                                           .AllowAnyMethod() 
                                           .AllowAnyHeader()));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
