using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration
      .SetBasePath(builder.Environment.ContentRootPath)      
      .AddJsonFile("ocelot.json")
      .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");
  builder.Services
      .AddOcelot(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.UseOcelot();
await app.RunAsync();