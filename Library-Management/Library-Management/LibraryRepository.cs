using Library_Management.Interfaces;
using Library_Management.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Library_Management;

public class LibraryRepository : ILibraryRepository
{
    private readonly string _filePath;
    private readonly ConcurrentDictionary<string, Book> _books = new();
    private readonly SemaphoreSlim _saveSemaphore = new(1, 1);

    public LibraryRepository(string filePath)
    {
        _filePath = filePath;
        LoadBooks();
    }

    public void LoadBooks()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var bookList = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
            foreach (var book in bookList)
            {
                _books.TryAdd(book.Id, book);
            }
        }
    }

    public async Task AddAsync(Book newBook)
    {
        if (_books.TryAdd(newBook.Id, newBook))
        {
            await SaveChangesAsync();
        }
    }

    public Book? GetBookById(string id)
    {
        return _books.TryGetValue(id, out var book) ? book : null;
    }

    public async Task UpdateAsync(Book updatedBook)
    {
        _books.AddOrUpdate(updatedBook.Id, updatedBook, (key, oldBook) => updatedBook);
        await SaveChangesAsync();
    }

    public async Task DeleteBookAsync(string id)
    {
        if (_books.TryRemove(id, out _))
        {
            await SaveChangesAsync();
        }
    }

    public List<Book> GetAvailableBooks()
    {
        var availableBooks = _books.Values.Where(c => c.BookStatus == BookStatus.Available).ToList();
        return availableBooks;
    }

    public List<Book> GetAllBooks()
    {
        return _books.Values.ToList();
    }

    public List<Book> SearchBooks(string query)
    {
        return _books.Values.Where(c => c.Title.Contains(query) || c.Author.Contains(query)).ToList();
    }

    public async Task SaveChangesAsync()
    {
        await _saveSemaphore.WaitAsync();
        try
        {
            var json = JsonConvert.SerializeObject(_books.Values, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _saveSemaphore.Release();
        }
    }
}
