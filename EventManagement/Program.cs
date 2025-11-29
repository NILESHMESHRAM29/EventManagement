using EventManagement.Data;
using EventManagement.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
<<<<<<< HEAD
// QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

// -------------------- PORT --------------------
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});
=======
=======
// ----------------------
// 1️⃣ Add CORS for Angular
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200", 
            "https://eventmanagement-lwxj.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
>>>>>>> 6db23ed7363b1e7b7446589ee6d77922a2a39e8c

// DB connection
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
>>>>>>> 41012e6d991e96a6633c8c48dba4e9213ed9ee79

// -------------------- DbContext --------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

<<<<<<< HEAD
// -------------------- Services --------------------
=======
>>>>>>> 41012e6d991e96a6633c8c48dba4e9213ed9ee79
builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IdCardPdfService>(); // Important!
builder.Services.AddControllers();
<<<<<<< HEAD

// -------------------- Swagger --------------------
=======
>>>>>>> 41012e6d991e96a6633c8c48dba4e9213ed9ee79
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Management API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter token like: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

<<<<<<< HEAD
<<<<<<< HEAD
// -------------------- JWT --------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");
var jwtIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Issuer");
var jwtAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Audience");
=======
=======
// JWT config
>>>>>>> 6db23ed7363b1e7b7446589ee6d77922a2a39e8c
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");
var jwtIssuer = jwtSettings["Issuer"];
var jwtAudience = jwtSettings["Audience"];

builder.Services.AddScoped<JwtService>();
>>>>>>> 41012e6d991e96a6633c8c48dba4e9213ed9ee79

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// -------------------- Build App --------------------
var app = builder.Build();

// Developer exception page for detailed errors
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

// ----------------------
// 2️⃣ Enable CORS (MUST be before Authentication)
// ----------------------
app.UseCors("AngularPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Simple test endpoint
app.MapGet("/", () => "Event Management API is running...");

app.Run();
