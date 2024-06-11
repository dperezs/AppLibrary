using DataAccess.Services.Books;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class TransientConfig
    {
        public TransientConfig(IServiceCollection services) {
            services.AddTransient<IBookRepository, BookRepository>();
        }
    }
}
