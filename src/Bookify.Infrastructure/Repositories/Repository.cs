using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;

internal abstract class Repository<T>(
    ApplicationDbContext dbContext)
    where T : Entity
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public virtual void Add(T user) =>
        DbContext.Add(user);
}