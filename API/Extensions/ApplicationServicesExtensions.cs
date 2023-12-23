using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // services.AddDbContext<StoreContext> sets up the StoreContext class to be used as a service in the application. It is a dependency injection service that can be used in the constructor of other classes.
        // This means that whenever a class needs to use the StoreContext class, it can be passed in as a parameter in the constructor.
        // It then instantiates the StoreContext class and passes it in.
        services.AddDbContext<StoreContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            // opt.UseSqlite("Data Source=store.db"); configures the StoreContext class to use the SQLite database. The connection string is stored in the appsettings.json file.
        });

        // services.AddScoped<IProductRepository, ProductRepository>(); registers the ProductRepository class as a service in the application.
        // Why use AddScoped at all : AddScoped creates a new instance of the specified type for every request. This is the default lifetime for all services registered using the AddScoped method.
        // if we didn't use AddScoped, then the ProductRepository class would be instantiated once when the application starts and then the same instance would be used for every request.
        // This is not what we want. We want a new instance of the ProductRepository class to be created for every request.
        // The IProductRepository interface is used to register the ProductRepository class as a service. This is because the ProductRepository class implements the IProductRepository interface.
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // registers the GenericRepository class as a service in the application.
                                                                                       // The IGenericRepository interface is used to register the GenericRepository class as a service. This is because the GenericRepository class implements the IGenericRepository interface.
                                                                                       // since we don't know the type (hence the use of generics and empty <>) of the GenericRepository class, we use the typeof keyword to get the type of the GenericRepository class.

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // registers the MappingProfiles class as a service in the application.

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;


    }

}
