using Meeting_Room_Booking_API.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Meeting_Room_Booking_API.Middleware;
using Meeting_Room_Booking_API.Infrastructure;
using Meeting_Room_Booking_API.Infrastructure.Seeding;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

// Load environment variables from .env file at the very start
DotNetEnv.Env.Load();

// ── Serilog Configuration ──────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.File(new JsonFormatter(), "logs/booking-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Meeting Room Booking API...");

    var builder = WebApplication.CreateBuilder(args);

    // Use Serilog as the logging provider
    builder.Host.UseSerilog();

    // Required for CorrelationId enrichment
    builder.Services.AddHttpContextAccessor();

// ── Controllers ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── CORS Policy ────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["CORS_ORIGIN"] ?? "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ── Rate Limiting ──────────────────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    // Specific limit for sensitive auth endpoints
    options.AddFixedWindowLimiter("AuthLimit", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueLimit = 0;
    });
    
    // Global catch-all limit for everything else
    options.AddFixedWindowLimiter("GlobalLimit", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 0;
    });

    options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
});

// ── Swagger / OpenAPI ──────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Meeting Room Booking API",
        Version = "v1",
        Description = "REST API for managing meeting rooms and time-slot bookings."
    });

    // Include XML doc comments in Swagger UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Add JWT Bearer security scheme so Swagger UI shows an Authorize button
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {your JWT token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

// ── JWT Authentication ─────────────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT_ISSUER"] ?? "https://localhost:5001",
            ValidAudience = builder.Configuration["JWT_AUDIENCE"] ?? "https://localhost:5001",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT_KEY"]
                    ?? "ThisIsASecretKeyForTestingPurposesOnly123!"))
        };
    });

builder.Services.AddAuthorization();

// ── Application (Services) ───────────────────────────────────────────────────
builder.Services.AddApplication();

// ── Infrastructure (EF Core, Repositories) ─────────────────────────────────────
builder.Services.AddInfrastructure();

// ── Build ──────────────────────────────────────────────────────────────────────
var app = builder.Build();

// 1. Security Headers (Apply to ALL responses)
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});

// 2. Global Error Handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed the database with dummy data on first startup
await DatabaseSeeder.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meeting Room Booking API v1"));
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("GlobalLimit");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


// Ensure the Program class is accessible from the test project
public partial class Program { }