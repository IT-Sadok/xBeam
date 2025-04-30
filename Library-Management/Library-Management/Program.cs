using System;
using Library_Management;
using Library_Management.Interfaces;

internal class Program
{
    static void Main(string[] args)
    {
        ILibraryRepository repository = new LibraryRepository();
        LibraryService libService = new LibraryService(repository);

        while (true)
        {
            Console.WriteLine("\n -*- You are in the Library! -*-");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Remove a book");
            Console.WriteLine("3. Search a book");
            Console.WriteLine("4. Show all books");
            Console.WriteLine("5. Show available books");
            Console.WriteLine("6. Borrow a book");
            Console.WriteLine("7. Return a book");
            Console.WriteLine("8. Exit");
            Console.Write("Make your move: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Title: ");
                    string title = Console.ReadLine();
                    Console.Write("Author: ");
                    string author = Console.ReadLine();
                    Console.Write("Year of Release: ");
                    int year = int.Parse(Console.ReadLine());
                    Console.WriteLine(libService.AddBook(title, author, year).Message);
                    break;

                case "2":
                    Console.Write("ID of the book: ");
                    string removeId = Console.ReadLine();
                    Console.WriteLine(libService.RemoveBook(removeId).Message);
                    break;

                case "3":
                    Console.Write("Enter the Title or the Author: ");
                    string query = Console.ReadLine();
                    var foundBooks = libService.SearchBooks(query);
                    if (foundBooks.Count < 1)
                    { Console.WriteLine("-*- Sorry, we can't find such book. Try something else -*-"); break; }
                    Console.WriteLine("What we found:");
                    foundBooks.ForEach(b => Console.WriteLine($"Code: {b.Id}. Author: {b.Author}. Title: {b.Title}. Year:{b.YearRelease}. Status: {b.BookStatus}"));
                    break;

                case "4":
                    var allBooks = libService.GetAllBooks();
                    Console.WriteLine("What we found:");
                    allBooks.ForEach(b => Console.WriteLine($"Code: {b.Id}. Author: {b.Author}. Title: {b.Title}. Year:{b.YearRelease}. Status: {b.BookStatus}"));
                    break;

                case "5":
                    var availableBooks = libService.GetAvailableBooks();
                    if (availableBooks.Count < 1)
                    { Console.WriteLine("-*- Sorry, we don't have available bookd right now -*-"); break; }
                    Console.WriteLine("Available books:");
                    availableBooks.ForEach(b => Console.WriteLine($"Code: {b.Id}. Author: {b.Author}. Title: {b.Title}. Year:{b.YearRelease}"));
                    break;

                case "6":
                    Console.Write("ID of the book to borrow: ");
                    string borrowId = Console.ReadLine();
                    Console.WriteLine(libService.BorrowBook(borrowId).Message);
                    break;

                case "7":
                    Console.Write("ID of the book for return: ");
                    string returnId = Console.ReadLine();
                    Console.WriteLine(libService.ReturnBook(returnId).Message);
                    break;

                case "8":
                    return;

                default:
                    Console.WriteLine("-*- Wrong choice. Try one more time. -*-");
                    break;
            }
        }
    }
}
