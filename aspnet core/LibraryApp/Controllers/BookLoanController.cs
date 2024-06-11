using AutoMapper.Internal.Mappers;
using Contract.BookLoans;
using Contract.Books;
using EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookLoanController : ControllerBase
    {
        private LibraryContext _context;
        

        public BookLoanController(LibraryContext context)
        {
            _context = context;            

        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<BookLoanDto>> Get()
        {
            
            IEnumerable<BookLoanDto> data = null;
            IEnumerable<BookLoanDto> parametros = null;
            var mapper = MapperConfig.InitializeAutomapper();
            string message = null;
            try
            {
                var result = await _context.BookLoans.Include(x=>x.Book).ToListAsync();
                parametros = mapper.Map<List<BookLoan>, List<BookLoanDto>>(result);
                if (parametros == null)
                {
                    return NotFound(new { data, message = "No se encontró la información solicitada" });
                }
                data = parametros;
            }
            catch (Exception ex)
            {

                return NotFound(new { data, message = "Error" + ex.Message.ToString() });
            }
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new { data, message });

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookLoan>> Get(int id)
        {
            var bookItems = await _context.BookLoans.FindAsync(id);
            if (bookItems == null)
            {
                return NotFound();
            }

            return bookItems;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<BookLoanDto>> Post([FromBody] BookLoanDto data)
        {
            string message = null;

            var book = await _context.Books.FindAsync(data.BookId);

            if (book != null && ValidaExistencia(book)) {
                var bookLoan = new BookLoan(book, data.Person, data.Status);
                _context.BookLoans.Add(bookLoan);
                await _context.SaveChangesAsync();
                return CreatedAtAction(
                nameof(Get),
                new { id = bookLoan.Id },
                    bookLoan);
            }
            else
                return NotFound(new { data, message = "No hay existencia de libros." });

        }

        // PUT api/<ValuesController>/5
        [HttpPatch]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] BookLoanDto data, long id)
        {
            string message = null;

            if (id != data.Id)
            {
                return NotFound(new { data, message = "Id  no coincide con Id del Objeto o Formulario " + "Valor entidad: " + id + ", Valor parametro: " + data.Id });
            }

            var bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null)
            {
                return NotFound(new { data, message = "Id  no existe en la base de datos o no está asociado a un ID del Libro " });
            }

            var book = await _context.Books.FindAsync(data.BookId);
            bookLoan.AddUpdateBookLoan(book, data.Person, data.Status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookLoanItemExists(id))
            {
                return NotFound();
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new { data, message = message });
        }
        private bool BookLoanItemExists(long id)
        {
            return _context.BookLoans.Any(e => e.Id == id);
        }
        private bool ValidaExistencia(Book book)
        {
            var loan = _context.BookLoans.Any(e => e.Book.Id == book.Id);

            if (loan!=null) {
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
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var todoItem = await _context.BookLoans.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.BookLoans.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
