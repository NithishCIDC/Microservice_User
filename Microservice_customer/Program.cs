using Customer.Application.Interface;
using Customer.Application.Mapper;
using Customer.infrastructure.Data;
using Customer.infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
