using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkCore
{
    public class BookLoan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Book Book { get; set; }
        public string Person { get; set; }
        public string Status { get; set; }

        public BookLoan() { }

        public BookLoan(Book book, string person, string status) {
            AddUpdateBookLoan(book,person,status);
        }

        public BookLoan AddUpdateBookLoan(Book book, string person, string status)
        {
            if (string.IsNullOrEmpty(person) || string.IsNullOrWhiteSpace(person)) throw new Exception("Person is required");
            if (string.IsNullOrEmpty(person) || string.IsNullOrWhiteSpace(status)) throw new Exception("Status is required");

            Book = book;
            Person = person;
            Status = status;

            return this;
        }
    }
}
