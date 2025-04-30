using Library_Management.Models;

namespace Library_Management.Interfaces;

public interface ILibraryRepository
{
    List<Book> SearchBooks(string query);
    List<Book> GetAvailableBooks();
    List<Book> GetAllBooks();
    void Add(Book book);
    Book GetBookById(string id);
    void Update(Book book);
    void DeleteBook(string id);
    void SaveChanges();
}
