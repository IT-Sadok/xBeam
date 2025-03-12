using Library_Management.Interfaces;
using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management
{
    public class LibraryRepository : ILibraryRepository
    {
        public void AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void BorrowBook(string id)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAvailableBooks()
        {
            throw new NotImplementedException();
        }

        public void RemoveBook(string id)
        {
            throw new NotImplementedException();
        }

        public void ReturnBook(string id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public List<Book> SearchBooks(string query)
        {
            throw new NotImplementedException();
        }
    }
}
