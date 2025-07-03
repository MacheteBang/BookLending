using MacheteBang.BookLending.Books.DataStore;
using MacheteBang.BookLending.Users.DataStore;
using MacheteBang.BookLending.Users.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace MacheteBang.BookLending.Tests.Integration;

public class BookLendingWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _uniqueDbId;

    public BookLendingWebApplicationFactory(string uniqueDbId)
    {
        _uniqueDbId = uniqueDbId;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registrations
            RemoveDbContextRegistration<BooksDbContext>(services);
            RemoveDbContextRegistration<UsersDbContext>(services);

            // Register new in-memory databases with unique names
            services.AddDbContext<BooksDbContext>(options =>
                options.UseInMemoryDatabase($"BooksTestDb_{_uniqueDbId}"));

            services.AddDbContext<UsersDbContext>(options =>
                options.UseInMemoryDatabase($"UsersTestDb_{_uniqueDbId}"));

            // Ensure services are built and seeding is performed
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var userDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            SeedUsersAsync(userDbContext, userManager, roleManager).GetAwaiter().GetResult();
        });
    }

    private static void RemoveDbContextRegistration<T>(IServiceCollection services) where T : DbContext
    {
        var optionsDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (optionsDescriptor is not null)
            services.Remove(optionsDescriptor);

        var optionsConfigurationDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<T>));
        if (optionsConfigurationDescriptor is not null)
            services.Remove(optionsConfigurationDescriptor);

        var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (contextDescriptor is not null)
            services.Remove(contextDescriptor);
    }

    private static async Task SeedUsersAsync(
        UsersDbContext dbContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        // Ensure a clean state
        await dbContext.Database.EnsureCreatedAsync();

        // Seed roles
        await roleManager.CreateAsync(new Role { Name = Constants.Roles.Administrator });
        await roleManager.CreateAsync(new Role { Name = Constants.Roles.Member });

        // Seed users
        var adminEmail = Constants.Users.AdminEmail;
        var memberEmail = Constants.Users.MemberEmail;

        var adminUser = new User { UserName = adminEmail, Email = adminEmail };
        await userManager.CreateAsync(adminUser, Constants.Users.Password);
        await userManager.AddToRoleAsync(adminUser, Constants.Roles.Administrator);

        var memberUser = new User { UserName = memberEmail, Email = memberEmail };
        await userManager.CreateAsync(memberUser, Constants.Users.Password);
        await userManager.AddToRoleAsync(memberUser, Constants.Roles.Member);
    }
}
