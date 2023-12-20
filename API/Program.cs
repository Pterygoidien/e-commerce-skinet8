using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<StoreContext> sets up the StoreContext class to be used as a service in the application. It is a dependency injection service that can be used in the constructor of other classes.
// This means that whenever a class needs to use the StoreContext class, it can be passed in as a parameter in the constructor.
// It then instantiates the StoreContext class and passes it in.
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    // opt.UseSqlite("Data Source=store.db"); configures the StoreContext class to use the SQLite database. The connection string is stored in the appsettings.json file.
});

// builder.Services.AddScoped<IProductRepository, ProductRepository>(); registers the ProductRepository class as a service in the application.
// Why use AddScoped at all : AddScoped creates a new instance of the specified type for every request. This is the default lifetime for all services registered using the AddScoped method.
// if we didn't use AddScoped, then the ProductRepository class would be instantiated once when the application starts and then the same instance would be used for every request.
// This is not what we want. We want a new instance of the ProductRepository class to be created for every request.
// The IProductRepository interface is used to register the ProductRepository class as a service. This is because the ProductRepository class implements the IProductRepository interface.
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
