using AutoMapper;
using Contract.BookLoans;
using Contract.Books;
using EntityFrameworkCore;

namespace LibraryApp
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                //Configuring Employee and EmployeeDTO
                cfg.CreateMap<Book, BookDto>();
                cfg.CreateMap<BookLoan, BookLoanDto>()
                .ForMember(x => x.BookName, opt => opt.MapFrom(src => src.Book.Name));
                //Any Other Mapping Configuration ....
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
