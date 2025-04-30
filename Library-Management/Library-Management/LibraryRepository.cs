using Library_Management.Interfaces;
using Library_Management.Models;
using Newtonsoft.Json;

namespace Library_Management;

public class LibraryRepository : ILibraryRepository
{
    private readonly string _filePath = Path.Combine(Environment.CurrentDirectory, "Library.json"); 
    private List<Book> _books;

    public LibraryRepository ()
    { 
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
        }
        else
        {
            _books = new List<Book>(); 
            using (FileStream fs = File.Create(_filePath))
            using (StreamWriter jsonStream = new StreamWriter(fs))
            {
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(jsonStream, _books);
            }
        }    
    }
    
    public ServiceResponse<Book> AddBook(Book book)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            _books.Add(book);
            response.Data = book;
            response.Message = "-*- The book is added successfully -*-";
            SaveChanges();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<Book> BorrowBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                response.Message = "-*- There are no book with such code -*-";
                response.Success = false;
                return response;
            }

            if (book.BookStatus == BookStatus.Borrowed)
            {
                response.Message = "-*- This book is already taken -*-";
                response.Success = false;
                return response;
            }

            book.BookStatus = BookStatus.Borrowed;
            response.Message = "-*- You have succesfully borrowed the book -*-";
            SaveChanges();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public List<Book> GetAvailableBooks()
    {
        var availableBooks = _books.Where(c=>c.BookStatus == BookStatus.Available).ToList();
        return availableBooks;
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }

    public List<Book> SearchBooks(string query)
    {
        return _books.Where(c => c.Title.Contains(query) || c.Author.Contains(query)).ToList();
    }

    public ServiceResponse<Book> RemoveBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                response.Message = "-*- There are no book with such code -*-";
                response.Success = false;
                return response;
            }

            _books.Remove(book);

            book.BookStatus = BookStatus.Borrowed;
            response.Message = "-*- The book has been succesfully removed! -*-";
            SaveChanges();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public ServiceResponse<Book> ReturnBook(string id)
    {
        var response = new ServiceResponse<Book>();

        try
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
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
            _books.Remove(book);

            book.BookStatus = BookStatus.Available;
            response.Message = "-*- Thank you for returning the book -*-";
            SaveChanges();
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = $"Error: {ex.Message}";
        }

        return response;
    }

    public void SaveChanges()
    {
        var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}
