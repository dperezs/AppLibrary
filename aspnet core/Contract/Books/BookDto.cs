using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Books
{
    public class BookDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NoCopies { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        
    }
}
