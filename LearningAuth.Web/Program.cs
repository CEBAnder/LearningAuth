using FluentMigrator.Runner;
using LearningAuth.Data;
using LearningAuth.Data.Migrations;
using LearningAuth.Data.Repositories;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DbContext>();
builder.Services.AddSingleton<Database>();
builder.Services
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("SqlConnection"))
        .ScanIn(typeof(Migration_1_AddUserTable).Assembly).For.Migrations());

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.MigrateDatabase();

app.Run();