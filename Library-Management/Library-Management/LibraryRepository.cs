using Library_Management.Interfaces;
using Library_Management.Models;
using Newtonsoft.Json;

namespace Library_Management
{
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
        
        public string AddBook(Book book)
        {
            _books.Add(book);
            SaveChanges();
            return "-*- The book is added! -*-";
        }

        public string BorrowBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                return "-*- There are no book with such code -*-"; 
            }

            if (book.BookStatus == BookStatus.Borrowed)
            {
                return "-*- This book is already taken -*-";
            }

            book.BookStatus = BookStatus.Borrowed;
            SaveChanges();
            return "-*- You have succesfully borrowed the book -*-";
        }

        public List<Book> GetAvailableBooks()
        {
            var availableBooks = _books.Where(c=>c.BookStatus == BookStatus.Available).ToList();
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

        public string RemoveBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
                return "-*- There are no book with such code -*-";

            _books.Remove(book);
            SaveChanges();
            return "-*- The book has been removed! -*-";
        }

        public string ReturnBook(string id)
        {
            var book = _books.FirstOrDefault(c => c.Id == id);
            if (book == null)
                return "-*- There are no book with such code -*-";

            if (book.BookStatus == BookStatus.Available)
                return "-*- This book is still available -*-";

            book.BookStatus = BookStatus.Available;
            SaveChanges();
            return "-*- Thank you for returning the book -*-";
        }

        public void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_books, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
