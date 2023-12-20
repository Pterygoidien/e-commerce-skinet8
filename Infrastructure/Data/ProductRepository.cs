using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private StoreContext _storeContext;

    public ProductRepository(StoreContext storeContext)
    {
        this._storeContext = storeContext;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
        return await _storeContext.ProductBrands.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _storeContext.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductType)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _storeContext.Products
        .Include(p => p.ProductBrand)
        .Include(p => p.ProductType)
        .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
        return await _storeContext.ProductTypes.ToListAsync();
    }
}
