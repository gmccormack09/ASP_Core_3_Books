using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{

    public class SQLEmployeeRepository : IBookRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Book Add(Book book)
        {
            context.Book.Add(book);
            context.SaveChanges();
            return book;
        }

        public Book Delete(int Id)
        {
            Book book = context.Book.Find(Id);
            if (book != null)
            {
                context.Book.Remove(book);
                context.SaveChanges();
            }
            return book;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return context.Book;
        }

        public Book GetBook(int Id)
        {
            return context.Book.Find(Id);
        }

        public Book Update(Book bookChanges)
        {
            var book = context.Book.Attach(bookChanges);
            book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bookChanges;
        }
    }

}
