using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EntityFrameworkCore
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>(b =>
            {
                b.ToTable("Books");                
            });
            builder.Entity<BookLoan>(b =>
            {
                b.ToTable("BookLoans");
                b.HasOne(x => x.Book).WithMany(y => y.BookLoans);                
                
            });
        }
        
    }
}
