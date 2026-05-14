//using Common;
//using EmailSystem;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.RateLimiting;
//using Microsoft.IdentityModel.Tokens;
//using Rentelligence.AI.Extensions;
//using Repository;
//using RepositoryContract;
//using Serilog;
//using System.Text;
//using System.Threading.RateLimiting;

//var builder = WebApplication.CreateBuilder(args);

//// Connection String
//var connectionString = builder.Configuration.GetConnectionString("DbCon");

////  CORS - Allow All
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

//// Services
//builder.Services.AddScoped<EmailService>();
//builder.Services.AddControllers();

////  JWT Authentication
//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(
//            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

//builder.Services.AddAuthorization();

////  Rate Limiting
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("api", opt =>
//    {
//        opt.Window = TimeSpan.FromMinutes(1);
//        opt.PermitLimit = 30;
//        opt.QueueLimit = 0;
//    });
//});
////builder.Services.AddScoped<TransactionsLogRepository>();
//builder.Services.AddScoped<ITransactionsLogRepository, TransactionsLogRepository>();
////  Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////  Custom DI
//builder.Services.addDapperContext();
//builder.Services.ConfigureRepositoryManager();
//builder.Services.ConfigureServiceManager();
//builder.Services.ConfigureLoggerServce();
//builder.Services.AddHttpContextAccessor();

//// Build
//var app = builder.Build();

////  Swagger Auth Middleware
//app.UseWhen(context => context.Request.Path.StartsWithSegments("/xoxofxapis"), appBuilder =>
//{
//    appBuilder.UseMiddleware<SwaggerAuthMiddleware>();
//});

////  Swagger UI
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "XoxoFx API V1");
//    c.RoutePrefix = "xoxofxapis";
//});

//// Middleware Order (IMPORTANT)
//app.UseHttpsRedirection();

//app.UseCors("AllowAll");   // ✅ FIXED

//app.UseRateLimiter();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
using Common;
using EmailSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rentelligence.AI.Extensions;
using Repository;
using RepositoryContract;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DbCon");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Services
builder.Services.AddScoped<EmailService>();
builder.Services.AddControllers();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        // IMPORTANT
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 30;
        opt.QueueLimit = 0;
    });
});

// Repository
builder.Services.AddScoped<ITransactionsLogRepository, TransactionsLogRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "XoxoFx API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Custom DI
builder.Services.addDapperContext();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerServce();
builder.Services.AddHttpContextAccessor();

// Build
var app = builder.Build();

// Swagger Auth Middleware
app.UseWhen(context => context.Request.Path.StartsWithSegments("/xoxofxapis"), appBuilder =>
{
    appBuilder.UseMiddleware<SwaggerAuthMiddleware>();
});

// Swagger
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "XoxoFx API V1");
    c.RoutePrefix = "xoxofxapis";
});

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();