using AutoMapper;
using Contract.BookLoans;
using Contract.Books;
using EntityFrameworkCore;

namespace LibraryApp
{
    public class LibraryAppAutoMapperProfile : Profile
    {
        public LibraryAppAutoMapperProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookLoan, BookLoanDto>()
            .ForMember(x=>x.BookName, opt => opt.MapFrom(src => src.Book.Name));
        }
    }
}
