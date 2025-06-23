namespace MacheteBang.BookLending.Books.DataStore;

internal sealed class BooksDbContext(DbContextOptions<BooksDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookCopy> BookCopies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Isbn value object to be stored as a string
        modelBuilder.Entity<Book>()
            .Property(b => b.Isbn)
            .HasConversion(
                isbn => isbn.Value,
                value => Isbn.Create(value)
            );

        // Configure Book and BookCopy relationship
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Copies)
            .WithOne(bc => bc.Book)
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure BookCopy entity
        modelBuilder.Entity<BookCopy>()
            .HasKey(bc => bc.BookCopyId);
    }
}
