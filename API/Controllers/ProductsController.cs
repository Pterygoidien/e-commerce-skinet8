using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private IGenericRepository<Product> _productRepository;
    private IGenericRepository<ProductBrand> _productBrandRepository;
    private IGenericRepository<ProductType> _productTypeRepository;
    private IMapper _mapper;

    public ProductsController(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductBrand> productBrandRepository,
        IGenericRepository<ProductType> productTypeRepository,
        IMapper mapper
    )
    {
        this._productRepository = productRepository;
        this._productBrandRepository = productBrandRepository;
        this._productTypeRepository = productTypeRepository;
        this._mapper = mapper;
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
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(
        string sort,
        int? brandId,
        int? typeId
    )
    {
        var spec = new ProductsWithTypesAndBrandsSpecifications(sort, brandId, typeId);
        var products = await _productRepository.ListAsync(spec);
        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
    }



    /// <summary>
    /// Get a product by id
    /// </summary>
    /// <param name="id">The id (int) of the product</param>
    /// <returns>
    /// Returns the product with the specified id if found, or null if not found.
    /// </returns>
    /// <response code="200">Returns the product with the specified id if found, or null if not found.</response>
    /// <response code="204">If the product is not found (null)</response>
    [HttpGet("{id}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecifications(id);

        var product = await _productRepository.GetEntityWithSpec(spec);

        if (product == null) return NotFound(new ApiResponse(404));

        return _mapper.Map<Product, ProductToReturnDto>(product);

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


    /// <summary>
    /// Get a list of product brands
    /// </summary>
    /// <returns>
    /// Returns a list of product brands.
    /// </returns>
    /// <response code="200">Returns a list of product brands.</response>
    /// <response code="204">If the product brands list is empty</response>
    [HttpGet("brands", Name = "GetProductBrands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepository.ListAllAsync());
    }


    /// <summary>
    /// Get a list of product types
    /// </summary>
    /// <returns>
    /// Returns a list of product types.
    /// </returns>
    /// <response code="200">Returns a list of product types.</response>
    /// <response code="204">If the product types list is empty</response>
    [HttpGet("types", Name = "GetProductTypes")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        var productTypes = await _productTypeRepository.ListAllAsync();
        return Ok(productTypes);
    }
}

