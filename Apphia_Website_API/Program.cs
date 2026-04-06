using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Apphia_Website_API.Repository;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.Configuration;
using Apphia_Website_API.Repository.Configuration.Helper;
using Apphia_Website_API.Repository.Configuration.Exception_Extender;
using Apphia_Website_API.Repository.Service.AuditLog;
using Apphia_Website_API.Extension;
using Apphia_Website_API.Utils;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// Add services to the container.
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IRequestStatusHelper, RequestStatusHelper>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

builder.Services.AddScoped<IAesService>(provider => {
    var config = provider.GetService<IConfiguration>() ?? throw new Exception("IConfiguration not found");
    return new AesService(config["AesKey"] ?? throw new Exception("AesKey not found"));
});

builder.Services.AddCors(options => {
    options.AddPolicy("FrontEnd", builder => {
        builder.WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents {
        OnMessageReceived = context => {
            var accessToken = context.HttpContext.Request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;
            return Task.CompletedTask;
        }
    };
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableDateOnlyJsonConverter());
    });
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<HttpContextSettings>(builder.Configuration.GetSection("HttpContextSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database contexts
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Audit services
builder.Services.AddAuditServices();
builder.Services.AddSingleton<IPaginationService, PaginationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Static files for GrapesJS images
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "grapesjs")),
    RequestPath = "/assets/contents/images/grapesjs",
    OnPrepareResponse = ctx => {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        ctx.Context.Response.Headers.Append("Vary", "Origin");
    }
});

// Static files for product images
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "products")),
    RequestPath = "/assets/contents/images/products",
    OnPrepareResponse = ctx => {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        ctx.Context.Response.Headers.Append("Vary", "Origin");
    }
});

// Static files for gallery images
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "contents", "images", "gallery")),
    RequestPath = "/assets/contents/images/gallery",
    OnPrepareResponse = ctx => {
        var origin = ctx.Context.Request.Headers["Origin"].ToString();
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", origin);
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
        ctx.Context.Response.Headers.Append("Vary", "Origin");
    }
});


app.UseMiddleware<GlobalException>();
app.UseHttpsRedirection();
app.UseCors("FrontEnd");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
