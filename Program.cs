using BookManagementSystem.Helpers;

namespace BookManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Welcome to our Library Management System");
                Console.WriteLine("You can do the following operations in our application:");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Operations that can be done by the librarian:");
                Console.WriteLine("1. Add a book to the library");
                //Console.WriteLine("6. Display the borrowed books");
                Console.WriteLine();
                Console.WriteLine("Operations that can be done by the clients");
                Console.WriteLine("2. Borrow a book from library");
                Console.WriteLine("3. Return a book to the library");
                Console.WriteLine("4. Display availability for a specific book");
                Console.WriteLine("5. Display all books in the library");
                Console.WriteLine("6. Exit library");
                Console.WriteLine();

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        string title = ValidationHelper.ReadNonEmptyStringValue("Enter book title: ");
                        string isbn = ValidationHelper.ReadNonEmptyStringValue("Enter ISBN: ");
                        decimal rentalPrice = ValidationHelper.ReadPositiveDecimalValue("Enter rental price: ");
                        int availableCopies = ValidationHelper.ReadPositiveIntValue("Enter number of available copies: ");
                        library.AddBook(title, isbn, rentalPrice, availableCopies);
                        break;                  
                    case "2":
                        string borrowTitle = ValidationHelper.ReadNonEmptyStringValue("Enter book title: ");
                        string user = ValidationHelper.ReadNonEmptyStringValue("Enter your Name: ");
                        DateTime borrowdate = ValidationHelper.IsDateValid("Enter the borrow date using this format dd/MM/yyyy: ");
                        library.BorrowBook(borrowTitle, user, borrowdate);
                        break;
                    case "3":
                        string returnTitle = ValidationHelper.ReadNonEmptyStringValue("Enter book title: ");
                        string username = ValidationHelper.ReadNonEmptyStringValue("Enter your Name: ");
                        DateTime returndate = ValidationHelper.IsDateValid("Enter the return date using this format dd/MM/yyyy: ");
                        library.ReturnBook(returnTitle, username, returndate);
                        break;
                    case "4":
                        string availableTitle = ValidationHelper.ReadNonEmptyStringValue("Enter book title: ");
                        library.AvailableCopiesInLibrary(availableTitle);
                        break;
                    case "5":
                        var allBooks = library.GetAllBooks();
                        if(allBooks.Count() == 0)
                        {
                            Console.WriteLine("There are no books in the library.");
                        }
                        else
                        {
                            Console.WriteLine("All books in the library:");
                            foreach (var book in allBooks)
                            {
                                Console.WriteLine($"Title: {book.Title}, ISBN: {book.ISBN}, Available Copies: {book.AvailableCopies}, Total Copies: {book.TotalCopies}, Price: {book.RentalPrice.ToString("F2")}");
                            }
                        }
                        break;
                    //case "6":
                    //    var borrowedbooks = library.GetAllBorrowedBooks();
                    //    if (borrowedbooks.Count() == 0)
                    //    {
                    //        Console.WriteLine("There are no borrowed books in the library.");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("All borrowed books in the library:");
                    //        foreach (var borrowedbook in borrowedbooks)
                    //        {
                    //            Console.WriteLine($"Title: {borrowedbook.Title}, ISBN: {borrowedbook.ISBN}, Borrowed By: {borrowedbook.UserName}, Date of Borrow: {borrowedbook.BorrowDate.ToString("dd/MM/yyyy")}");
                    //        }
                    //    }
                    //    break;
                    case "6":
                        string option = ValidationHelper.ReadStringQuestionResponse("Are you sure you want to exit the library: Y/N ");
                        if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            exit = true;
                        }
                        else if (option.Equals("N", StringComparison.OrdinalIgnoreCase))
                        {
                            exit = false;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}