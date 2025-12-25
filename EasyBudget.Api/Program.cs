using EasyBudget.Api.Data;
using Microsoft.EntityFrameworkCore;
using EasyBudget.Api.Exceptions;
using EasyBudget.Api.Services;

// Load environment variables

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load(".env.local");

var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
ArgumentException.ThrowIfNullOrEmpty(dbName, "DATABASE_NAME environment variable is not set");
var dbUser = Environment.GetEnvironmentVariable("DATABASE_USER");
ArgumentException.ThrowIfNullOrEmpty(dbUser, "DATABASE_USER environment variable is not set");
var dbPass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
ArgumentException.ThrowIfNullOrEmpty(dbPass, "DATABASE_PASSWORD environment variable is not set");

var connectionString = $"Host=localhost;Database={dbName};Username={dbUser};Password={dbPass}";

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

builder.Services.AddScoped<ITellerService, TellerService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
