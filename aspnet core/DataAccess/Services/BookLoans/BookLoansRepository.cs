using EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.BookLoans
{
    public class BookLoansRepository : IBookLoansRepository
    {
        private LibraryContext _context;
        public BookLoansRepository(LibraryContext context) {
            _context = context;
        }
        public async Task<IEnumerable<BookLoan>> GetAll()
        {
            return await _context.BookLoans.Include(x => x.Book).ToListAsync();
        }
        public async Task<BookLoan> GetById(long id)
        {
            return await _context.BookLoans.FindAsync(id);
        }
        public bool ValidaExistencia(Book book) {
            var loan = _context.BookLoans.Any(e => e.Book.Id == book.Id);

            if (loan != null)
            {
                var queryable = _context.BookLoans.Where(x => x.Book.Id == book.Id && x.Status == "Prestamo").GroupBy(x => x.Book.Id)

                .Select(x => new
                {
                    x.Key,
                    books = x.Count()
                }).FirstOrDefault();

                return queryable != null ? queryable?.books < book.NoCopies : true;
            }
            return false;
        }
        public bool BookLoanItemExists(long id)
        {
            return _context.BookLoans.Any(e => e.Id == id);
        }
        public async Task Add(BookLoan entity)
        {
            _context.BookLoans.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task Update(BookLoan entity)
        {
            var book = _context.BookLoans.Where(x => x.Id == entity.Id).FirstOrDefault();
            book.AddUpdateBookLoan(entity.Book, entity.Person, entity.Status);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(BookLoan entity)
        {
            _context.BookLoans.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
