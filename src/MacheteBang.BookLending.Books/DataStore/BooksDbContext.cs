using Microsoft.EntityFrameworkCore;

namespace MacheteBang.BookLending.Books.DataStore;

internal sealed class BooksDbContext(DbContextOptions<BooksDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}
