using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using User.Application.Interface;
using User.Application.Mapper;
using User.infrastructure.Data;
using User.infrastructure.Repository;

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

#endregion

#region AddHttpClient

builder.Services.AddHttpClient("Product", options =>
{
    options.BaseAddress = new Uri("https://localhost:7115/api/");
});

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

#region JWT Role Policy

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

#endregion

#region Serilog

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

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
