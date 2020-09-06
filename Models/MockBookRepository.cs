using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class MockBookRepository : IBookRepository
    {
        private List<Book> _bookList;

        public MockBookRepository()
        {
            _bookList = new List<Book>(){
                new Book() { Id = 1, Name = "Blood Meridian", Author = "Cormac McCarthy", Genre = Genre.Western, PhotoPath = "~/images/BloodMeridian.png" },
                new Book() { Id = 2, Name = "Biography of Malcolm X", Author= "Malcolm X", Genre = Genre.Autobiography, PhotoPath= "~/images/MalcolmXBook.png"},
                new Book() { Id = 3, Name = "Beyond Good and Evil", Author= "Friedrich Nietzsche", Genre = Genre.Philosophy}
            };
        }

        public Book Add(Book book)
        {
            book.Id = _bookList.Max(e => e.Id) + 1;
            _bookList.Add(book);
            return book;
        }

        public Book Delete(int Id)
        {
            Book book = _bookList.FirstOrDefault(e => e.Id == Id);
            if (book != null)
            {
                _bookList.Remove(book);
            }
            return book;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookList;
        }

        public Book GetBook(int Id)
        {
            return _bookList.FirstOrDefault(e => e.Id == Id);
        }

        public Book Update(Book bookChanges)
        {
            Book book = _bookList.FirstOrDefault(e => e.Id == bookChanges.Id);
            if (book != null)
            {
                book.Name = bookChanges.Name;
                book.Author = bookChanges.Author;
                book.Genre = bookChanges.Genre;
                book.PhotoPath = bookChanges.PhotoPath;
            }
            return book;
        }
    }
}
