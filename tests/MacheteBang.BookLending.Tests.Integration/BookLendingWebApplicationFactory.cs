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
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registrations
            RemoveDbContextRegistration<BooksDbContext>(services);
            RemoveDbContextRegistration<UsersDbContext>(services);

            // Add InMemory database contexts
            services.AddDbContext<BooksDbContext>(options =>
            {
                options.UseInMemoryDatabase("BooksTestDb");
            });

            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseInMemoryDatabase("UsersTestDb");
            });

            // Make sure Identity services are registered properly for seeding
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var userDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            // Ensure database is created
            userDbContext.Database.EnsureCreated();

            // Create roles immediately after database creation
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var adminRole = new Role { Name = "Administrator" };
                roleManager.CreateAsync(adminRole).Wait();
            }

            if (!roleManager.RoleExistsAsync("Member").Result)
            {
                var memberRole = new Role { Name = "Member" };
                roleManager.CreateAsync(memberRole).Wait();
            }
        });
    }

    private static void RemoveDbContextRegistration<T>(IServiceCollection services) where T : DbContext
    {
        var optionsDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (optionsDescriptor != null) services.Remove(optionsDescriptor);

        var optionsConfigurationDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<T>));
        if (optionsConfigurationDescriptor != null) services.Remove(optionsConfigurationDescriptor);

        var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
        if (contextDescriptor != null) services.Remove(contextDescriptor);
    }
}
