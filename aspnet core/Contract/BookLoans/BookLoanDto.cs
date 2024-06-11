using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.BookLoans
{
    public class BookLoanDto
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string Person { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
