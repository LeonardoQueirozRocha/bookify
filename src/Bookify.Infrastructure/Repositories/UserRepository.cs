using Bookify.Domain.Users.Entities;
using Bookify.Domain.Users.Interfaces;
using Bookify.Infrastructure.Context;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext dbContext) 
    : Repository<User>(dbContext), IUserRepository;