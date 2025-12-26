using EasyBudget.Api.Data;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Exceptions;
using EasyBudget.Api.Services;
using EasyBudget.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load(".env.local");

var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
ArgumentException.ThrowIfNullOrEmpty(dbName, "DATABASE_NAME environment variable is not set");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
ArgumentException.ThrowIfNullOrEmpty(dbUser, "DATABASE_USER environment variable is not set");
var dbPass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
ArgumentException.ThrowIfNullOrEmpty(dbPass, "DATABASE_PASSWORD environment variable is not set");

var connectionString = $"Host=localhost;Database={dbName};Username={dbUser};Password={dbPass}";

var auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN");
ArgumentException.ThrowIfNullOrEmpty(auth0Domain, "AUTH0_DOMAIN environment variable is not set");
var auth0Audience = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE");
ArgumentException.ThrowIfNullOrEmpty(auth0Audience, "AUTH0_AUDIENCE environment variable is not set");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{auth0Domain}/";
    options.Audience = auth0Audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddDataProtection();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        // Add trace id so clients can correlate errors with server logs
        context.ProblemDetails.Extensions.TryAdd("traceId", context.HttpContext.TraceIdentifier);
    };

});

builder.Services.AddLogging();

var tellerBaseUrl = Environment.GetEnvironmentVariable("TELLER_BASE_URL");
ArgumentException.ThrowIfNullOrEmpty(tellerBaseUrl, "TELLER_API_BASE_URL environment variable is not set");
builder.Services.AddHttpClient("Teller", client =>
{
    client.BaseAddress = new Uri(tellerBaseUrl);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("easy-budget");
});


// Make sure certain handlers are registered in the desired order
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<TellerService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EnrollmentService>();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
