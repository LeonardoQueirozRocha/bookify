using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    
    public void Add(T entity) => 
        DbContext.Add(entity);
}