using AutoMapper;
using EventManagement.Data;
using EventManagement.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var builder = WebApplication.CreateBuilder(args);

// -------------------- QUESTPDF LICENSE --------------------
QuestPDF.Settings.License = LicenseType.Community;

// -------------------- CORS --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://eventmanagement-lwxj.onrender.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// -------------------- DATABASE --------------------
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
    .EnableSensitiveDataLogging()     
    .EnableDetailedErrors()

);

// -------------------- SERVICES --------------------
builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IdCardPdfService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// -------------------- SWAGGER --------------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Event Management API",
        Version = "v1"
    });

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

// -------------------- JWT AUTH --------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");
var jwtIssuer = jwtSettings["Issuer"];
var jwtAudience = jwtSettings["Audience"];

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

// -------------------- BUILD APP --------------------
var app = builder.Build();

// ---------- ORDER MATTERS ----------
app.UseCors("AngularPolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
