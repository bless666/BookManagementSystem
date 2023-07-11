using BookManagementSystem.Models;
using BookManagementSystem.Helpers;

namespace BookManagementSystem
{
    public class Library
    {
        public List<Book> books;
        public List<BorrowedBook> borrowedbooks;

        public Library()
        {
            books = new List<Book>();
            borrowedbooks = new List<BorrowedBook>();
        }

        /// <summary>
        /// Adds a new book to the book collection. 
        /// If a book with the same title already exists, it updates the number of available copies and, 
        /// if necessary, the ISBN and rental price.
        /// </summary>
        /// <param name="title">The title of the book.</param>
        /// <param name="isbn">The ISBN of the book.</param>
        /// <param name="rentalPrice">The rental price of the book.</param>
        /// <param name="availableCopies">The number of available copies of the book.</param>
        public void AddBook(string title, string isbn, decimal rentalPrice, int availableCopies)
        {
            Book existingBook = books.FirstOrDefault(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (existingBook == null) 
            {
                Book newBook = new Book(title, isbn, rentalPrice, availableCopies);
                books.Add(newBook);
                Console.WriteLine($"New book '{newBook.Title}' added. Available copies: {newBook.AvailableCopies}");
                return;
            }

            existingBook.AvailableCopies += availableCopies;
            existingBook.TotalCopies += availableCopies;

            //Treat the case when ISBN and Rental Price are not the same as the existing ones
            if(existingBook.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase) && existingBook.RentalPrice == rentalPrice)
            {
                Console.WriteLine($"Existing book '{existingBook.Title}' updated. Available copies: {existingBook.AvailableCopies}");
                return;
            }

            string option = ValidationHelper.ReadStringQuestionResponse("The book already exists with a diferent ISBN or Rental Price. \nDo you want to update them ? Y / N ");
            if (option.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                existingBook.ISBN = isbn;
                existingBook.RentalPrice = rentalPrice;
                Console.WriteLine("The ISBN and the Rental Price are updated.");
            }
            else if (option.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The ISBN and the Rental Price remain the same.");
            }
            Console.WriteLine($"Existing book '{existingBook.Title}' updated. Available copies: {existingBook.AvailableCopies}");     
        }

        /// <summary>
        /// Borrows a book from the library for a given user, 
        /// recording the borrowing details and updating the book's availability.
        /// </summary>
        /// <param name="title">The title of the book to borrow.</param>
        /// <param name="username">The username of the borrower.</param>
        /// <param name="borrowdate">The date of borrowing.</param>
        public void BorrowBook(string title,string username, DateTime borrowdate)
        {
            var book = books.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (book == null) 
            {
                Console.WriteLine($"Book '{title}' not found in the library.");
                return;
            }

            if(book.AvailableCopies == 0)
            {
                Console.WriteLine($"Book '{book.Title}' is not available for borrowing.");
                return;
            }

            //Add a new entry into BorrowedBooks List
            var borrowedbook = new BorrowedBook(book.Title, book.ISBN, book.RentalPrice, book.AvailableCopies, borrowdate, username, true, 0.0m);
            borrowedbooks.Add(borrowedbook); 
            book.AvailableCopies--;
            Console.WriteLine($"Book '{book.Title}' borrowed successfully.");
            return;
        }


        /// <summary>
        /// Returns a borrowed book to the library, updates the book's availability,
        /// and calculates any applicable penalties for late returns.
        /// </summary>
        /// <param name="title">The title of the book to return.</param>
        /// <param name="username">The username of the borrower.</param>
        /// <param name="returndate">The date of return.</param>
        public void ReturnBook(string title, string username, DateTime returndate)
        {
            var book = books.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (book == null)
            {
                Console.WriteLine($"Book '{title}' not found in the library.");
                return;
            }

            var borrowedbook = borrowedbooks.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            if(borrowedbook == null)
            {
                Console.WriteLine($"No one with the name '{username}' have borrowed this book.");
                return;
            }

            if(borrowedbook.Borrowed == false)
            {
                Console.WriteLine($"You have already returned the book to the library.");
                return;
            }

            //Calculate the number of days between borrowdate and returndate
            TimeSpan timeSpan = returndate.Subtract(borrowedbook.BorrowDate);
            int numberOfDays = timeSpan.Days;

            if(numberOfDays < 0)
            {
                Console.WriteLine($"The return date must be the same or greater then borrowed date. \n The book was borrowed on {borrowedbook.BorrowDate.ToString("dd/MM/yyyy")}");
                return;
            }

            book.AvailableCopies++; 
            borrowedbook.Borrowed = false; 
            decimal penalty = 0;
            int daysOverdue = numberOfDays - 14; // Calculate the number of days beyond 2 weeks

            //Calculate the penalty if the book was returned after more then 2 weeks
            if (daysOverdue > 0) 
            {
                penalty = daysOverdue * (book.RentalPrice * 0.01m);
                borrowedbook.Penalty = penalty;
            }
            Console.WriteLine($"Book '{book.Title}' returned successfully.");
            if (penalty > 0)
            {
                Console.WriteLine($"Penalty for {daysOverdue} days overdue: {penalty:C}");
            }
        }

        //Retrieves all the books existing in the library
        public List<Book> GetAllBooks()
        {
            return books;
        }

        // Retrieves a book from the library based on its title.
        public Book GetBook(string title)
        {
            var book = books.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (book != null)
            {
                return book;
            }
            return null;
        }

        // Checks the availability of copies for a book in the library.
        public void AvailableCopiesInLibrary(string title)
        {
            foreach (Book book in books)
            {
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    if (book.AvailableCopies > 0)
                    {
                        Console.WriteLine($"Book '{title}' has {book.AvailableCopies} copies available.");
                    }
                    else
                    {
                        Console.WriteLine($"Book '{title}' doesn't have any copies available.");
                    }
                    return;
                }
            }
            Console.WriteLine($"Book '{title}' not found in the library.");
        }

        //Retrieves the number of available copies in the library. Used for test porposes
        public int GetAvailableCopies(string title)
        {
            foreach (Book book in books)
            {
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    return book.AvailableCopies;
                }
            }
            return 0;
        }

        //Returns all the books existing in the library
        //public List<BorrowedBook> GetAllBorrowedBooks()
        //{
        //    return borrowedbooks.Where(x => x.Borrowed == true).ToList();
        //}
    }
}