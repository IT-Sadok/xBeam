using Library_Management.Models;

namespace Library_Management.Interfaces
{
    public interface ILibraryRepository
    {
        void AddBook(Book book);
        void RemoveBook(string id);
        List<Book> SearchBooks(string query);
        List<Book> GetAvailableBooks();
        void BorrowBook(string id);
        void ReturnBook(string id);
        void SaveChanges();
    }
}
