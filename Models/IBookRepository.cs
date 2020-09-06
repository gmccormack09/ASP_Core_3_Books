using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public interface IBookRepository
    {
        Book GetBook(int Id);
        IEnumerable<Book> GetAllBooks();
        Book Add(Book book);
        Book Update(Book bookChanges);
        Book Delete(int Id);
    }
}
