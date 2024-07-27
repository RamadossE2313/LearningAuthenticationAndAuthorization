using LearningAuthenticationAndAuthorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Sample value
//"Jwt": {
//"Key": "2dGhY6z4QaL8bTx9N3pRfWvAqZ1cXsVe5jUm7yPdLoI9Kn0hBsC4MxEwJiYtFu",
//"Issuer": "LearningAuthenticationAndAuthorizationApi"
//}
// Key value is stored in user secret

//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddControllers();

// if we didn't enable cors, browser can't connect with api resources, it will throw cors error
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", options =>
    {
        options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenProviderService, TokenProviderService>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors("EnableCORS");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
