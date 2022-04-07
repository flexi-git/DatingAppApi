using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;



// Add MyDbContext to Dependency Injection
builder.Services.AddDbContext<ApiDataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Transient); //// this will reset your model to its original value if you decided to cancel the operations.

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
