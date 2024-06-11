using EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.BookLoans
{
    public interface IBookLoansRepository
    {
        Task<IEnumerable<BookLoan>> GetAll();
        Task<BookLoan> GetById(long id);
        bool ValidaExistencia(Book book);
        bool BookLoanItemExists(long id);
        Task Add(BookLoan entity);
        Task Update(BookLoan entity);
        Task Delete(BookLoan entity);
    }
}
