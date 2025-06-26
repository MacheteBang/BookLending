namespace MacheteBang.BookLending.Users.Responses;

public sealed record UserResponse(
    Guid UserId,
    string UserName,
    string Email,
    bool EmailConfirmed,
    string? PhoneNumber,
    bool PhoneNumberConfirmed,
    bool TwoFactorEnabled,
    DateTimeOffset? LockoutEnd
);

public sealed record UsersResponse(int Count, List<UserResponse> Users);

public static class UserResponseExtensions
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse(
            user.Id,
            user.UserName,
            user.Email,
            user.EmailConfirmed,
            user.PhoneNumber,
            user.PhoneNumberConfirmed,
            user.TwoFactorEnabled,
            user.LockoutEnd
        );
    }

    public static UsersResponse ToResponse(this ICollection<User> users)
    {
        return new UsersResponse(
            users.Count,
            users
                .Select(u => u.ToResponse())
                .ToList()
        );
    }
}
