using Library_Management.Interfaces;
using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management
{
    public class LibraryService
    {
        private readonly ILibraryRepository _repository;

        public  LibraryService(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public string AddBook(string title, string author, int YearRelease)
        {
            var book = new Book { Id = Guid.NewGuid().ToString(), Title = title, Author = author, YearRelease = YearRelease };
            return _repository.AddBook(book);
        }

        public string RemoveBook (string id)
        {
            return _repository.RemoveBook(id);
        }

        public List<Book> GetAvailableBooks() => _repository.GetAvailableBooks();

        public List<Book> GetAllBooks() => _repository.GetAllBooks();


        public List<Book> SearchBooks(string query) => _repository.SearchBooks(query);

        public string BorrowBook(string id) =>  _repository.BorrowBook(id);

        public string ReturnBook(string id) => _repository.ReturnBook(id);

    }
}
