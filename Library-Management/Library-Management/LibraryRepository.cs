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
    
    public void Add(Book newBook)
    {
        try
        {
            _books.Add(newBook);
            SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Book GetBookById(string id)
    {
        try
        {
            return _books.FirstOrDefault(c => c.Id == id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Update(Book book)
    {
        try
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
        catch (Exception)
        {
            throw;
        }
    }

    public void DeleteBook(string id)
    {
        try
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
                SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
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
