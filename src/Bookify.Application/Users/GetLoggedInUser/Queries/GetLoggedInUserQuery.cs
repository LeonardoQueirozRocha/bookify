using Bookify.Application.Abstractions.Messaging.Queries;
using Bookify.Application.Users.GetLoggedInUser.Responses;

namespace Bookify.Application.Users.GetLoggedInUser.Queries;

public record GetLoggedInUserQuery : IQuery<UserResponse>;