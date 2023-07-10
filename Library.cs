using LibraryApplication.Models;

namespace LibraryApplication
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

        //Adds a new book to the library or updates an existing one
        public void AddBook(string title, string isbn, decimal rentalPrice, int availableCopies)
        {
            Book existingBook = books.FirstOrDefault(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (existingBook != null)
            {
                existingBook.AvailableCopies += availableCopies;
                existingBook.TotalCopies += availableCopies;

                //Treat the case when ISBN and Rental Price are not the same as the existing ones
                if (!existingBook.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase) || existingBook.RentalPrice != rentalPrice)
                {
                    Console.WriteLine();
                    string option = Helper.Helper.ReadStringQuestionResponse("The book already exists with a diferent ISBN or Rental Price. \nDo you want to update them ? Y / N");
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
                else
                {
                    Console.WriteLine($"Existing book '{existingBook.Title}' updated. Available copies: {existingBook.AvailableCopies}");
                }
            }
            else
            {
                Book newBook = new Book(title, isbn, rentalPrice, availableCopies, availableCopies);
                books.Add(newBook);
                Console.WriteLine($"New book '{newBook.Title}' added. Available copies: {newBook.AvailableCopies}");
            }
        }

        //Returns all the books existing in the library
        public List<Book> GetAllBooks()
        {           
            return books;
        }

        //Returns all the books existing in the library
        //public List<BorrowedBook> GetAllBorrowedBooks()
        //{
        //    return borrowedbooks.Where(x => x.Borrowed == true).ToList();
        //}

        //Returns the number of available copies in the library. Used for test porposes
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

        //Shows how many copies of a book exists in the library
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

        //Borrow a book from library
        public void BorrowBook(string title,string username, DateTime borrowdate)
        {
            foreach (Book book in books)
            {
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    if (book.AvailableCopies > 0)
                    {
                        //Add a new entry into BorrowedBooks List
                        var borrowedbook = new BorrowedBook(book.Title, book.ISBN, book.RentalPrice, book.AvailableCopies);
                        borrowedbook.Borrowed = true;
                        borrowedbook.UserName = username;
                        borrowedbook.BorrowDate = borrowdate;
                        borrowedbooks.Add(borrowedbook); 

                        book.AvailableCopies--;
                        Console.WriteLine($"Book '{book.Title}' borrowed successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Book '{book.Title}' is not available for borrowing.");
                    }
                    return;
                }
            }
            Console.WriteLine($"Book '{title}' not found in the library.");
        }


        //Return a book to the library
        public void ReturnBook(string title, string username, DateTime returndate)
        {
            foreach (Book book in books)
            {
                if (book.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    var borrowedbook = borrowedbooks.FirstOrDefault(x => x.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

                    if(borrowedbook != null && borrowedbook.Borrowed == true) 
                    {
                        //Calculate the number of days between borrowdate and returndate
                        TimeSpan timeSpan = returndate.Subtract(borrowedbook.BorrowDate);
                        int numberOfDays = timeSpan.Days;

                        if(numberOfDays >= 0) 
                        {
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
                        else
                        {
                            Console.WriteLine($"The return date must be the same or greater then borrowed date. The book was borrowed on {borrowedbook.BorrowDate.ToString("dd/MM/yyyy")}");
                        }
                        
                    }
                    else if(borrowedbook != null && borrowedbook.Borrowed == false) 
                    {
                        Console.WriteLine($"You have already returned the book to the library.");
                    }
                    else 
                    {
                        Console.WriteLine($"No one with the name '{username}' have borrowed this book.");
                    }
                    return;
                }
            }
            Console.WriteLine($"Book '{title}' not found in the library.");
        }
    }
}
