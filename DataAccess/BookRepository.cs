using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class BookRepository : IBookRepository
    {
        private readonly  LibraryContext _context;
        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetEmployees()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<Book> GetEmployee(int id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
