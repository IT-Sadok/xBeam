using Library_Management.Models;

namespace Library_Management.Interfaces;

public interface ILibraryService
{
    ServiceResponse<Book> AddBook(string title, string author, int YearRelease);
    ServiceResponse<Book> RemoveBook(string id);
    List<Book> GetAvailableBooks();
    List<Book> GetAllBooks();
    List<Book> SearchBooks(string query);
    ServiceResponse<Book> BorrowBook(string id);
    ServiceResponse<Book> ReturnBook(string id);
}
