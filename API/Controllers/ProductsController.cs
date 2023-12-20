using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _storeContext;

    public ProductsController(StoreContext storeContext)
    {
        this._storeContext = storeContext; // this is a reference to the current instance of the class

    }

    /// <summary>
    /// Get a list of products
    /// </summary>
    /// <returns>
    /// Returns a list of products.   
    /// </returns>
    [HttpGet(Name = "GetProducts")]
    // ActionResult : Represents the result of an action method. An action method returns an ActionResult object or one of its many derived types.
    // List<Product> : Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await _storeContext.Products.ToListAsync();
    }



    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id">The id (UUID) of the product</param>
    /// <returns>
    /// Returns the product with the specified id if found, or null if not found.
    /// </returns>
    /// <response code="200">Returns the product with the specified id if found, or null if not found.</response>
    /// <response code="204">If the product is not found (null)</response>
    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        return await _storeContext.Products.FindAsync(id);
    }


    /// <summary>
    /// Add a product
    /// </summary>
    /// <returns></returns>
    [HttpPost(Name = "AddProduct")]
    public string AddProduct()
    {
        return "This will add a product";
    }


    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="id">The id (UUID) of the product</param>
    /// <returns></returns>
    /// <response code="200">Returns the updated product</response>
    /// <response code="400">If the product is null</response>
    [HttpPut("{id}", Name = "UpdateProduct")]
    public string UpdateProduct(int id)
    {
        return $"This will update product {id}";
    }


    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">The id (UUID) of the product</param>
    /// <returns></returns>
    /// <response code="200">Returns the deleted product</response>
    /// <response code="400">If the product is null</response>
    [HttpDelete("{id}", Name = "DeleteProduct")]
    public string DeleteProduct(int id)
    {
        return $"This will delete product {id}";
    }
}