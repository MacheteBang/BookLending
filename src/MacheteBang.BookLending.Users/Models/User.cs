namespace MacheteBang.BookLending.Users.Models;

public class User : IdentityUser<Guid>
{
#nullable disable
    public override required string UserName { get; set; }
    public override required string Email { get; set; }
#nullable restore
}
