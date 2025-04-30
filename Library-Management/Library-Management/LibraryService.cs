using Library_Management.Interfaces;
using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _repository;

    public  LibraryService(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public ServiceResponse<Book> AddBook(string title, string author, int YearRelease)
    {
        var book = new Book { Id = Guid.NewGuid().ToString(), Title = title, Author = author, YearRelease = YearRelease };
        return _repository.AddBook(book);
    }

    public ServiceResponse<Book> RemoveBook (string id)
    {
        return _repository.RemoveBook(id);
    }

    public List<Book> GetAvailableBooks() => _repository.GetAvailableBooks();

    public List<Book> GetAllBooks() => _repository.GetAllBooks();


    public List<Book> SearchBooks(string query) => _repository.SearchBooks(query);

    public ServiceResponse<Book> BorrowBook(string id) =>  _repository.BorrowBook(id);

    public ServiceResponse<Book> ReturnBook(string id) => _repository.ReturnBook(id);

}
