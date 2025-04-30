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
        var response = new ServiceResponse<Book>();

        try
        {
            var newBook = new Book { Id = Guid.NewGuid().ToString(), Title = title, Author = author, YearRelease = YearRelease };

            var existing = _repository.GetBookById(newBook.Id);
            if (existing != null)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "-*- A book with this ID already exists -*-"
                };
            }

            _repository.Add(newBook);

            response.Data = newBook;
            response.Message = "-*- The book is added successfully -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<Book> RemoveBook (string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _repository.GetBookById(id);
            if (book == null)
            {
                response.Message = "-*- There are no book with such code -*-";
                response.Success = false;
                return response;
            }

            _repository.DeleteBook(id);

            response.Message = "-*- The book has been succesfully removed! -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public List<Book> GetAvailableBooks() => _repository.GetAvailableBooks();

    public List<Book> GetAllBooks() => _repository.GetAllBooks();

    public List<Book> SearchBooks(string query) => _repository.SearchBooks(query);

    public ServiceResponse<Book> BorrowBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _repository.GetBookById(id);

            if (book == null)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "-*- There is no book with such code -*-"
                };
            }

            if (book.BookStatus == BookStatus.Borrowed)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "-*- This book is already taken -*-"
                };
            }

            book.BookStatus = BookStatus.Borrowed;
            _repository.Update(book);
            response.Data = book;
            response.Message = "-*- You have successfully borrowed the book -*-";
        }
        catch (Exception ex)
        {
            return new ServiceResponse<Book>
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            };
        }

        return response;
    }

    public ServiceResponse<Book> ReturnBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _repository.GetBookById(id);
            if (book == null)
            {
                response.Message = "-*- There are no book with such code -*-";
                response.Success = false;
                return response;
            }
            if (book.BookStatus == BookStatus.Available)
            {
                response.Message = "-*- This book is still available -*-";
                response.Success = false;
                return response;
            }

            book.BookStatus = BookStatus.Available;
            _repository.Update(book);
            response.Message = "-*- Thank you for returning the book -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

}
