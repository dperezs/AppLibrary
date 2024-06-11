using AutoMapper;
using AutoMapper.Internal.Mappers;
using Contract.BookLoans;
using Contract.Books;
using DataAccess.Services.Books;
using EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _BookRepository;
        public BooksController(IBookRepository BookRepository)
        {
            _BookRepository = BookRepository;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<BookDto>> Get()
        {
            IEnumerable<BookDto> data = null;
            IEnumerable<BookDto> parametros = null;
            var mapper = MapperConfig.InitializeAutomapper();
            string message = null;
            try
            {
                var result = await _BookRepository.GetAll();
                parametros = mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(result);
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
        public async Task<ActionResult<Book>> Get(long id)
        {
            var bookItems =  await _BookRepository.GetById(id);
            if (bookItems == null)
            {
                return NotFound();
            }

            return bookItems;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromBody] BookDto data)
        {
            var book = new Book( data.Name, data.NoCopies, data.Description);
            var books = _BookRepository.GetByName(data.Name);
            if (books.Count()==0) {
                await _BookRepository.Add(book);
                return CreatedAtAction(
                nameof(Get),
                new { id = book.Id },
                    book);
            }else
                return NotFound(new { data, message = "El titulo del libro ya existe" });


        }

        // PATCH api/<ValuesController>/5
        [HttpPatch]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] BookDto data, long id )
        {
            string message = null;
            
            if (id != data.Id)
            {
                return NotFound(new { data, message = "Id  no coincide con Id del Objeto o Formulario " + "Valor entidad: " + id + ", Valor parametro: " + data.Id });
            }

            var book = await _BookRepository.GetById(id);
            if (book == null)
            {
                return NotFound(new { data, message = "Id  no existe en la base de datos o no está asociado a un ID del Libro " });
            }
            book.AddUpdateBook(data.Name, data.NoCopies, data.Description);
            

            try
            {
                await _BookRepository.Update(book);
            }
            catch (DbUpdateConcurrencyException) when (!_BookRepository.BookItemExists(id))
            {
                return NotFound();
            }

            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(new { data, message = message });
        }
        

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            string message = null;
            Book data = new Book();
            data = null;            
            var bookItems = await _BookRepository.GetById(id);
            if (bookItems == null)
            {
                return NotFound(new { data, message = "El registro no existe en la base de datos o no está asociado a un ID " });
            }
            try
            {
                await _BookRepository.Delete(bookItems);
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
