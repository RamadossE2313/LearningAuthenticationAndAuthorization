using LearningAuthenticationAndAuthorization.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

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


#region RequestLimit

#region QueueLimit = 0
//builder.Services.AddRateLimiter(_ => _
//.AddFixedWindowLimiter(policyName: "fixed", options =>
//{
//options.PermitLimit = 4;
//options.Window = TimeSpan.FromSeconds(30);
//options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//    // When request limit exceeds more then PermitLimit within the window,
//    // then if no status code configured, then default it returns as 503 (service unavailable)
//options.QueueLimit = 0;
//})); 
#endregion

#region QueueLimit = 2
//builder.Services.AddRateLimiter(_ => _
//.AddFixedWindowLimiter(policyName: "fixed", options =>
//{
//options.PermitLimit = 4;
//options.Window = TimeSpan.FromSeconds(30);
//options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//    // if we send 5 requests, 5th request will be processed after 30 seconds only
//    // if we send 8 requests, first 4 request will execute immediately,
//    // and 5 and 6 will be queued because queue limit set to 2,
//    // and 7 and 8 will return 503 (Service unavailable)
//options.QueueLimit = 2;
//}));
#endregion

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
     {
         options.PermitLimit = 4;
         options.Window = TimeSpan.FromSeconds(30);
         options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
         // if we send 5 requests, 5th request will be processed after 30 seconds only
         // if we send 8 requests, first 4 request will execute immediately,
         // and 5 and 6 will be queued because queue limit set to 2,
         // and 7 and 8 will return 429 (Too many requests)
         options.QueueLimit = 2;
     });

    options.RejectionStatusCode = 429;
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseCors("EnableCORS");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
