using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Ocelot.json", optional:false,reloadOnChange:true);
builder.Services.AddOcelot();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
