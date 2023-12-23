/**
* The program.cs is the entry point of the application. It is the first file that is run when the application starts.
*/

using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // this is middleware that will intercept any errors and redirect to the error controller
app.UseStatusCodePagesWithReExecute("/errors/{0}"); // this is middleware that will intercept any errors and redirect to the error controller
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy"); // this is middleware that will intercept any requests and redirect to the CorsPolicy
                           // the CorsPolicy is defined in the ApplicationServicesExtensions.cs file

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
