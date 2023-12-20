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

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _storeContext.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _storeContext.Products.ToListAsync();
    }
}
