using Library_Management.Models;

namespace Library_Management.Interfaces
{
    public interface ILibraryRepository
    {
        string AddBook(Book book);
        string RemoveBook(string id);
        List<Book> SearchBooks(string query);
        List<Book> GetAvailableBooks();
        List<Book> GetAllBooks();
        string BorrowBook(string id);
        string ReturnBook(string id);
        void SaveChanges();
    }
}
