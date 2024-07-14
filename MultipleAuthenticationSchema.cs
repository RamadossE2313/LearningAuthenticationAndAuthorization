using Microsoft.IdentityModel.Tokens;
using System.Text;
using LearningAuthenticationAndAuthorization.Common;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using LearningAuthenticationAndAuthorization.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.AddApiVersioning(version =>
{
    version.AssumeDefaultVersionWhenUnspecified = true;
    version.DefaultApiVersion = new ApiVersion(1, 0);
    version.ReportApiVersions = true;
    version.ApiVersionReader = new HeaderApiVersionReader("X-Version");
}).AddMvc();

// configure service collection
services.AddAuthentication()
    .AddCookie(Constants.COOKIESCHEMENAME)
    .AddJwtBearer(Constants.JWT1SCHEMENAME, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Constants.JWT1ISSUER,
            ValidAudience = Constants.JWT1AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JWT1SECRETKEY))
        };
    })
    .AddJwtBearer(Constants.JWT2SCHEMENAME, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Constants.JWT2ISSUER,
            ValidAudience = Constants.JWT2AUDIENCE,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.JWT2SECRETKEY))
        };
    });

services.AddAuthorization(options =>
{
    // default policy set for the 3 schemes,, if user use jw1, jw2, cookie all will support.
    // if we use [authorize], if anyone schema matches it will support automatically
    var defaultAuthorizationPolicy = new AuthorizationPolicyBuilder(Constants.JWT1SCHEMENAME, Constants.JWT2SCHEMENAME, Constants.COOKIESCHEMENAME);

    options.DefaultPolicy = defaultAuthorizationPolicy.RequireAuthenticatedUser().Build();

    // created policy to support only jwt1 and jwt2
    options.AddPolicy(Constants.JWTPOLICYENAME, new AuthorizationPolicyBuilder(Constants.JWT1SCHEMENAME, Constants.JWT2SCHEMENAME)
        .RequireAuthenticatedUser().Build());

    // created policy to support only jwt1 and jwt2
    options.AddPolicy(Constants.JWTADMINPOLICYNAME, new AuthorizationPolicyBuilder(Constants.JWT1SCHEMENAME, Constants.JWT2SCHEMENAME)
        .RequireAuthenticatedUser().RequireRole("Admin").Build());

    // created policy to support cookie only
    options.AddPolicy(Constants.COOKIEPOLICYNAME, new AuthorizationPolicyBuilder(Constants.COOKIESCHEMENAME)
     .RequireAuthenticatedUser().Build());

    // created policy to support only jwt1 scheme
    options.AddPolicy(Constants.JWT1POLICYENAME, new AuthorizationPolicyBuilder(Constants.JWT1SCHEMENAME)
     .RequireAuthenticatedUser().Build());

    // created policy to support only jwt2 scheme
    options.AddPolicy(Constants.JWT2POLICYENAME, new AuthorizationPolicyBuilder(Constants.JWT2SCHEMENAME)
     .RequireAuthenticatedUser().Build());

});

services.AddControllers();

services.AddScoped<IUserService, UserService>();
var app = builder.Build();


// configuring request pipeline
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
