using EntityFrameworkCore;

namespace DataAccess.Services.Books
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetById(long id);        
        Task Add(Book entity);
        Task Update(Book entity);
        Task Delete(Book entity);
        bool BookItemExists(long id);
        List<Book> GetByName(string name);
    }
}