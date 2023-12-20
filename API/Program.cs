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


//create a scope for the application, in order to manipulate services outside of an HTTP request
using var scope = app.Services.CreateScope(); // creates a scope for the application

// using keyword : the using keyword is used to define a scope for an object. When the scope ends, the object is disposed of.
// In this case, the scope ends when the application ends. The scope is used to create an instance of the StoreContext class.
// The StoreContext class is then used to create the database if it doesn't exist and apply any pending migrations.

// now, our AddDbContext up in the file is a scoped service. This is an extension service for IServiceCollection. This is scoped to the HTTP request.
// this CreateScope allows us to get access to that service inside our program class where we do not have the abiulity to inject a service inside there.
// so we can use the scope to get access to the service provider and then we can get access to the StoreContext.

// so, since earlier we defined a scope that can be accessed only from an HTTP request and we are not in an HTTP request, we need to create a scope to access the service provider inside program.cs, so outside an HTTP request. 
// we thus create a scope and then we get the service provider from that scope and then we get the StoreContext from that service provider.

var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

//try and migrate the database and if there is an error, log the error
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}


app.Run();
