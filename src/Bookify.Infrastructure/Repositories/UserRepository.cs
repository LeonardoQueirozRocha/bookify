using Bookify.Domain.Users.Entities;
using Bookify.Domain.Users.Interfaces;
using Bookify.Infrastructure.Context;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext)
    : Repository<User>(dbContext), IUserRepository
{
    public override void Add(User entity)
    {
        foreach (var role in entity.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(entity);
    }
}