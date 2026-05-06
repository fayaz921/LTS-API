using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
using LTS.API.Common.Behaviors;
using LTS.API.Common.DI;
using LTS.API.Domain.Entities;
using LTS.API.Infrastructure.BackgroundJobs;
using LTS.API.Infrastructure.Persistence;
using LTS.API.Infrastructure.Persistence.Extensions;
using LTS.API.Infrastructure.Services.CloudinaryFileStorage;
using LTS.API.Infrastructure.Services.Email;
using LTS.API.Infrastructure.Services.Extensions;
using LTS.API.Infrastructure.Services.JWT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173",
                          "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
         .AllowCredentials();
    });
});
// Swagger + JWT
//builder.Services.AddSwaggerDocumentation();
builder.Services.AddOpenApiWithJwt();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddJwtValidation(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Middleware pipelines
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

// Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
// Hangfire
builder.Services.AddHangfire(config =>
    config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseMemoryStorage());

builder.Services.AddHangfireServer();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseCors("AllowFrontend");
// All middleware (Swagger, Auth, Controllers, etc.)
app.MyMiddleWare();
// Hangfire Dashboard
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<HearingAlertJob>(
    "hearing-alert-job",
    job => job.ExecuteAsync(),
    "0 8 * * *"
);

await app.ApplyMigrationsAsync();

app.Run();