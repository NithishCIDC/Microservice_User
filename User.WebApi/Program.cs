using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using User.Application.Interface;
using User.Application.Mapper;
using User.Application.Validation;
using User.infrastructure.Data;
using User.infrastructure.Repository;
using User.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region CORS

builder.Services.AddCors(options => options.AddPolicy("CORS_Policy", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

#endregion

#region Database Connection

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Generics and Unit of work 

builder.Services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfwork>();
builder.Services.AddScoped<IUserProcessor, UserProcessor>();

#endregion

#region Fluent validation

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<EditUserValidator>();

#endregion

#region AddHttpClient and Httpcontext

builder.Services.AddHttpClient("Product", options =>
{
    options.BaseAddress = new Uri("https://localhost:7115/api/");
});

builder.Services.AddHttpContextAccessor();

#endregion

#region JWT_AUTH

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
       };
   });

#endregion

#region Serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() //info, warning, error, fatal
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(ApplicationMapper));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CORS_Policy");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
