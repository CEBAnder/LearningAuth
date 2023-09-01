using System.Text.Json;
using LearningAuth.Data;
using LearningAuth.Data.Repositories;
using LearningAuth.Web.Extensions;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("SqlConnection"));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddRateLimiting();

foreach (var c in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{c.Key}={c.Value}");
}

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders;
    options.RequestHeaders.Add("Authorization");
});

builder.Services.AddHealthChecks();

builder.Logging.AddJsonConsole(options =>
{
    options.IncludeScopes = true;
    options.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthChecks("/health");

app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseRateLimiter();

app.MigrateDatabase();

app.Run();