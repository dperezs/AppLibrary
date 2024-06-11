using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Services.Books
{
    public class BookRepository : IBookRepository
    {
        private LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Book>> GetAll()
        {

            return await _context.Books.ToListAsync();
        }
        public async Task<Book> GetById(long id)
        {
            return await _context.Books.FindAsync(id);
        }
        public List<Book> GetByName(string name)
        {
            var qry = _context.Books.AsQueryable();            
            var list = (from book in qry
                       where book.Name == name
                       select book).ToList();
            return  list;

        }
        public async Task Add(Book entity)
        {            
            _context.Books.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Book entity)
        {
            var book = _context.Books.Where(x=>x.Id == entity.Id ).FirstOrDefault();
            book.AddUpdateBook(entity.Name, entity.NoCopies, entity.Description);
            await _context.SaveChangesAsync();
        }
        public async  Task Delete(Book entity) {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public bool BookItemExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}