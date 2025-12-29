using EmailSystem;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rentelligence.AI.Extensions;
using Rentelligence.AI.Middleware;
using Repository;
using RepositoryContract;
using Serilog;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbCon");

// Allow CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
            "http://localhost:8081",
            "http://localhost:3000",
            "https://rentelligence.online",
            "https://rentelligence.ai",
            "https://app.rentelligence.ai",
            "https://ai-rentelligence.vercel.app",
            "https://ai-rentelligence-admin.vercel.app",
            "https://ai-rentelligence-home.vercel.app",
            "https://rentel-ai-market-place.vercel.app",
            "https://ai-market-place-admin.vercel.app/",
            "https://santrix-global-agentic.vercel.app/",
            "https://santrix-global-agentic-admin.vercel.app/",
            "https://santrx.com/",
            "https://apis.vibeworld.online/",
            "https://vibeworld.online/"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


// Swagger + MVC
builder.Services.AddControllersWithViews();

// JWT Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Dependency Injection
builder.Services.AddScoped<EmailService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Extensions
builder.Services.addDapperContext();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("Rentelligence", x =>
{
    x.BaseAddress = new Uri("Rentelligence.AI");
});

//  Firebase Initialization (Safe Mode)
try
{
    var firebasePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/firebase-service-account.json");

    if (File.Exists(firebasePath))
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(firebasePath)
        });

        Console.WriteLine(" Firebase initialized successfully.");
    }
    else
    {
        Console.WriteLine(" Firebase config file not found. Skipping Firebase initialization.");
    }
}
catch (Exception ex)
{
    Console.WriteLine(" Firebase initialization failed: " + ex.Message);
}

//  Build App
var app = builder.Build();

//  Swagger
app.UseSwagger();
app.UseSwaggerUI();

//  Exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

//  Static Files
app.UseStaticFiles();

//  MVC + API Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Portfolio}/{action=Index}/{id?}"
);

app.MapControllers();

app.Run();



