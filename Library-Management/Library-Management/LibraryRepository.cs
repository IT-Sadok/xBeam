using Library_Management.Interfaces;
using Library_Management.Models;
using Newtonsoft.Json;

namespace Library_Management;

public class LibraryRepository : ILibraryRepository
{
    private readonly string _filePath; 
    private List<Book> _books;

    public LibraryRepository (string filePath)
    { 
        _filePath = filePath;
        LoadBooks();
    }

    public void LoadBooks()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
        }
        else
        {
            _books = new List<Book>();
        }
    }

    public void Add(Book newBook)
    {
        _books.Add(newBook);
        SaveChanges();
    }

    public Book GetBookById(string id)
    {
        return _books.FirstOrDefault(c => c.Id == id);
    }

    public void Update(Book book)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook != null)
        {
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.YearRelease = book.YearRelease;
            existingBook.BookStatus = book.BookStatus;
        }
        SaveChanges();
    }

    public void DeleteBook(string id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _books.Remove(book);
            SaveChanges();
        }
    }

    public List<Book> GetAvailableBooks()
    {
        var availableBooks = _books.Where(c => c.BookStatus == BookStatus.Available).ToList();
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


    public void SaveChanges()
    {
        var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}
