using Library_Management.Interfaces;
using Library_Management.Models;
using System.Collections.Concurrent;

namespace Library_Management;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _repository;
    private static readonly ConcurrentDictionary<string, object> _bookLocks = new();

    public LibraryService(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public ServiceResponse<Book> AddBook(string title, string author, int yearRelease)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var newBook = new Book { Id = Guid.NewGuid().ToString(), Title = title, Author = author, YearRelease = yearRelease };

            var existing = _repository.GetBookById(newBook.Id);
            if (existing != null)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "-*- A book with this ID already exists -*-"
                };
            }

            _repository.AddAsync(newBook);

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
            response = CheckBookForNull(id, response);
            if (response.Success == false)
                return response;

            var book = response.Data;

            _repository.DeleteBookAsync(id);

            response.Message = "-*- The book has been succesfully removed! -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<List<Book>> GetAvailableBooks()
    {
        var books = _repository.GetAvailableBooks();
        return new ServiceResponse<List<Book>> { Data = books };
    }

    public ServiceResponse<List<Book>> GetAllBooks()
    {
        var allBooks = _repository.GetAllBooks();
        return new ServiceResponse<List<Book>> { Data = allBooks };
    }

    public ServiceResponse<List<Book>> SearchBooks(string query)
    {
        var books = _repository.SearchBooks(query);
        return new ServiceResponse<List<Book>> { Data = books };
    }

    public ServiceResponse<Book> BorrowBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            response = CheckBookForNull(id, response);
            if (response.Success == false)
                return response;

            var book = response.Data;

            if (book.BookStatus == BookStatus.Borrowed)
            {
                return new ServiceResponse<Book>
                {
                    Success = false,
                    Message = "-*- This book is already taken -*-"
                };
            }

            book.BookStatus = BookStatus.Borrowed;
            _repository.UpdateAsync(book);
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
            response = CheckBookForNull(id, response);
            if (response.Success == false)
                return response;

            var book = response.Data;
            if (book.BookStatus == BookStatus.Available)
            {
                response.Message = "-*- This book is still available -*-";
                response.Success = false;
                return response;
            }

            book.BookStatus = BookStatus.Available;
            _repository.UpdateAsync(book);
            response.Message = "-*- Thank you for returning the book -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<Book> EditBook(Book editBook, string title, string author, int yearRelease)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            editBook.Title = title;
            editBook.Author = author;
            editBook.YearRelease = yearRelease;
            _repository.UpdateAsync(editBook);
            response.Message = "-*- The book is successfully edited! -*-";
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<Book> GetBookById(string id)
    {
        var requestedBook = _repository.GetBookById(id);
        return new ServiceResponse<Book> { Data = requestedBook };
    }

    public ServiceResponse<Book> CheckBookForNull(string id, ServiceResponse<Book> response)
    {
        var requestedBook = _repository.GetBookById(id);

        if (requestedBook == null)
        {
            response.Message = "-*- There are no book with such code -*-";
            response.Success = false;
        }
        else
        {
            response.Data = requestedBook;
        }

        return response;
    }

    public ServiceResponse<Book> CheckBookBeforeEditing(string id)
    {
        ServiceResponse<Book> response = CheckBookForNull(id, new ServiceResponse<Book>());
        if (response.Success == false)
            return response;

        if (response.Data.BookStatus == BookStatus.Borrowed)
        {
            response.Message = "-*- You cannot edit the book while it is borrowed -*-";
            response.Success = false;
        }
        return response;
    }
}
