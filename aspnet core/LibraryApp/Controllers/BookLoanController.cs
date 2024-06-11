using AutoMapper.Internal.Mappers;
using Contract.BookLoans;
using Contract.Books;
using DataAccess.Services.BookLoans;
using DataAccess.Services.Books;
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
        private readonly IBookLoansRepository _BookLoansRepository;
        private readonly IBookRepository _BookRepository;


        public BookLoanController(IBookLoansRepository bookLoansRepository, IBookRepository BookRepository)
        {
            _BookLoansRepository = bookLoansRepository;
            _BookRepository = BookRepository;
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
                var result = await _BookLoansRepository.GetAll();
                parametros = mapper.Map<IEnumerable<BookLoan>, IEnumerable<BookLoanDto>>(result);
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
            var bookItems = await _BookLoansRepository.GetById(id);
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

            var book = await _BookRepository.GetById(data.BookId);

            if (book != null && _BookLoansRepository.ValidaExistencia(book)) {
                var bookLoan = new BookLoan(book, data.Person, data.Status);
                await _BookLoansRepository.Add(bookLoan); 
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

            var bookLoan = await _BookLoansRepository.GetById(id);
            if (bookLoan == null)
            {
                return NotFound(new { data, message = "Id  no existe en la base de datos o no está asociado a un ID del Libro " });
            }
            var book = await _BookRepository.GetById(data.BookId);
                        
            

            try
            {
                if (book != null && _BookLoansRepository.ValidaExistencia(book))
                {
                    bookLoan.AddUpdateBookLoan(book, data.Person, data.Status);
                    await _BookLoansRepository.Update(bookLoan);
                }
                else
                    return NotFound(new { data, message = "No hay existencia de libros." });
            }
            catch (DbUpdateConcurrencyException) when (!_BookLoansRepository.BookLoanItemExists(id))
            {
                return NotFound(new { data, message = "Existen registros de Libros utilizados en procesos del Sistema " });
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new { data, message = message });
        }
        
        
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            string message = null;
            BookLoan data = new BookLoan();
            data = null;

            var BookLoanItem = await _BookLoansRepository.GetById(id);
            if (BookLoanItem == null)
            {
                return NotFound(new { data, message = "El registro no existe en la base de datos o no está asociado a un ID " });
            }

            try
            {
                await _BookLoansRepository.Delete(BookLoanItem);
            }
            catch (System.Data.ConstraintException ex)
            {
                return NotFound(new { data, message = "Existen registros de Libros utilizados en procesos del Sistema " });
            }
            catch (Exception ex)
            {
                return NotFound(new { data, message = "Hubo un error en base de datos, notifique al administrador :  " + ex.Message.ToString() + "," + ex.StackTrace.ToString() });
            }



            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new { message });
        }
    }
}
