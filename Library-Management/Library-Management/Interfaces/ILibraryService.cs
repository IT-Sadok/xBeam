using Library_Management.Models;

namespace Library_Management.Interfaces;

public interface ILibraryService
{
    ServiceResponse<Book> AddBook(string title, string author, int YearRelease);
    ServiceResponse<Book> RemoveBook(string id);
    ServiceResponse<List<Book>> GetAvailableBooks();
    ServiceResponse<List<Book>> GetAllBooks();
    ServiceResponse<List<Book>> SearchBooks(string query);
    ServiceResponse<Book> BorrowBook(string id);
    ServiceResponse<Book> ReturnBook(string id);
}
