
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Shop.Application;
using Shop.Application.CustomLanguageMessage;
using Shop.Infrastructure;
using Shop.Infrastructure.Utilities;
using Shop.Persistence;
using Shop.Persistence.Context;
using Shop.SignalR;
using ShopAPI.Middlewares;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationPersistence.Initialize(builder.Configuration);
ConfigurationInfrastructure.Initialize(builder.Configuration);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(options =>
{
 options.RegisterModule(new AutofacBusinessModule());
});
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("Logs/_Log.txt", rollingInterval: RollingInterval.Day)
.MinimumLevel.Information()
.CreateLogger());

builder.Services.AddControllers();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop", Version = "v1", Description = "Identity Service API swagger client." });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\\"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllRole", policy =>
                     policy.RequireClaim(ClaimTypes.Role, "Admin", "SuperAdmin","User")); 
    options.AddPolicy("AdminRole", policy =>
                     policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("SuperAdminRole", policy =>
                 policy.RequireClaim(ClaimTypes.Role,"SuperAdmin"));
});
// JWT Auth
#region JWT Auth
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
            expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Email
    };
});
#endregion
#region Fluent Validation Registration add services to the container.
builder.Services
    .AddFluentValidation(configuration =>
    {
       
        configuration.DisableDataAnnotationsValidation = true;
        configuration.LocalizationEnabled = true;
        configuration.AutomaticValidationEnabled = false;
        configuration.DisableDataAnnotationsValidation = true;
        configuration.ValidatorOptions.LanguageManager = new CustomValidationLanguageMessage();
        configuration.ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");
    })
 ;

#endregion
var corsRuls = builder.Configuration.GetValue<string>("Domain:Front");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {

            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddRateLimiter(option =>
{
    option.AddFixedWindowLimiter("Fixed", _option =>
    {
        _option.Window = TimeSpan.FromSeconds(20);
        _option.PermitLimit = 20;
        _option.QueueLimit = 15;
        _option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;


    });
});

builder.Services.AddTransient<LocalizationMiddleware>();

var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseRateLimiter();
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapHubs();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LocalizationMiddleware>();
app.MapControllers();

app.Run();
