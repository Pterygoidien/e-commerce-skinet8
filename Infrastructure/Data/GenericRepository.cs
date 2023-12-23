using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private StoreContext _storeContext;

    public GenericRepository(StoreContext storeContext)
    {
        this._storeContext = storeContext;

    }
    public async Task<T> GetByIdAsync(int id)
    {
        return await _storeContext.Set<T>().FindAsync(id);
        // Set<T>() : Gets a DbSet<TEntity> for the given entity type. The DbSet<TEntity> for an entity type provides the
        // ability to perform create, read, update, and delete operations on entities of that type. DbSet<TEntity> objects are
        // usually obtained from a DbContext using the DbContext.Set method.
        // FindAsync : Asynchronously finds an entity with the given primary key values. If an entity with the given primary key
    }


    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _storeContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }
    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }
    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_storeContext.Set<T>().AsQueryable(), spec);
    }


}
