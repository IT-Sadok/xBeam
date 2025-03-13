using Library_Management.Interfaces;
using Library_Management.Models;
using Newtonsoft.Json;

namespace Library_Management
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly string _filePath = "library.json";
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
                _books = new List<Book> ();
            }    
        }
        
        public void AddBook(Book book)
        {
            _books.Add(book);
            SaveChanges();
        }

        public void BorrowBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                Console.WriteLine("There are no book with such code");
                return;
            }

            if (book.BookStatus == BookStatus.Borrowed)
            {
                Console.WriteLine("This book is already taken");
                return;
            }

            book.BookStatus = BookStatus.Borrowed;
            SaveChanges();
            Console.WriteLine("You have succesfully borrowed the book");
        }

        public List<Book> GetAvailableBooks()
        {
            var availableBooks = _books.Where(c=>c.BookStatus == BookStatus.Available).ToList();
            return availableBooks;
        }

        public List<Book> SearchBooks(string query)
        {
            return _books.Where(c => c.Title.Contains(query) || c.Author.Contains(query)).ToList();
        }

        public void RemoveBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                Console.WriteLine("There are no book with such code");
                return;
            }

            _books.Remove(book);
            SaveChanges();
            Console.WriteLine("You have succesfully removed the book");
        }

        public void ReturnBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                Console.WriteLine("There are no book with such code");
                return;
            }

            if (book.BookStatus == BookStatus.Available)
            {
                Console.WriteLine("This book is still available");
                return;
            }

            book.BookStatus = BookStatus.Available;
            SaveChanges();
            Console.WriteLine("Thank you for returning the book");
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
