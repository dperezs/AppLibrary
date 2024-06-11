using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int NoCopies { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public List<BookLoan> BookLoans { get; set; }
        public Book() { }

        public Book(string name, int noCopies, string description) {

            AddUpdateBook(name, noCopies, description);
        }

        public Book AddUpdateBook(string name, int noCopies, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required");
            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description)) throw new Exception("Description is required");

            Name = name;
            NoCopies = noCopies;
            Description = description;

            return this;
        }
    }
}
