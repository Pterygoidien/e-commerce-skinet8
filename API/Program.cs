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
