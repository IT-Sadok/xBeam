using Library_Management.Models;

namespace Library_Management.Interfaces;

public interface ILibraryRepository
{
    List<Book> SearchBooks(string query);
    List<Book> GetAvailableBooks();
    List<Book> GetAllBooks();
    Task AddAsync(Book book);
    Book GetBookById(string id);
    Task UpdateAsync(Book book);
    Task DeleteBookAsync(string id);
    Task SaveChangesAsync();
}
