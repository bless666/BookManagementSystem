using LibraryApplication;
using LibraryApplication.Models;

namespace LibraryTest
{
    [TestFixture]
    public class LibraryTests
    {
        //Unit Tests
        [Test]
        public void AddBook_WhenNewBookAdded_ShouldIncreaseAvailableCopies()
        {
            // Arrange
            Library library = new Library();
            string title = "Book1";
            string isbn = "ISBN123";
            decimal rentalPrice = 10.99m;
            int availableCopies = 3;

            // Act
            library.AddBook(title, isbn, rentalPrice, availableCopies);

            // Assert
            Assert.AreEqual(availableCopies, library.GetAvailableCopies(title));
        }

        [Test]
        public void AddBook_NewBook_ShouldAddToLibrary()
        {
            // Arrange
            Library library = new Library();

            // Act
            library.AddBook("Book 2", "ISBN-2", 12.0m, 3);

            // Assert
            int expectedAvailableCopies = 3;
            int actualAvailableCopies = library.GetAvailableCopies("Book 2");
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
        }

        [Test]
        public void AddBook_ExistingBook_ShouldUpdateAvailableCopies()
        {
            // Arrange
            Library library = new Library();
            library.AddBook("Book 1", "ISBN-1", 10.0m, 5);

            // Act
            library.AddBook("Book 1", "ISBN-1", 10.0m, 3);

            // Assert
            int expectedAvailableCopies = 8;
            int actualAvailableCopies = library.GetAvailableCopies("Book 1");
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
        }

        //Integration Tests

        [Test]
        public void BorrowBook_ExistingBookWithAvailableCopies_ShouldDecreaseAvailableCopies()
        {
            // Arrange
            Library library = new Library();
            library.AddBook("Book 3", "ISBN-3", 15.0m, 5);

            // Act
            library.BorrowBook("Book 3", "Adrian Razvan", DateTime.Now);

            // Assert
            int expectedAvailableCopies = 4;
            int actualAvailableCopies = library.GetAvailableCopies("Book 3");
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
        }

        [Test]
        public void BorrowBook_ExistingBookWithNoAvailableCopies_ShouldNotChangeAvailableCopies()
        {
            // Arrange
            Library library = new Library();
            library.AddBook("Book 4", "ISBN-4", 20.0m, 0);

            // Act
            library.BorrowBook("Book 4", "Adrian Razvan", DateTime.Now);

            // Assert
            int expectedAvailableCopies = 0;
            int actualAvailableCopies = library.GetAvailableCopies("Book 4");
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
        }


        //Functional Tests
        [Test]
        public void ReturnBook_ExistingBookWithinTwoWeeks_ShouldIncreaseAvailableCopiesAndNoPenalty()
        {
            // Arrange
            Library library = new Library();
            library.AddBook("Book 5", "ISBN-5", 25.0m, 2);

            // Act
            library.BorrowBook("Book 5", "Adrian Razvan", DateTime.Now);
            library.ReturnBook("Book 5", "Adrian Razvan", DateTime.Now.AddDays(10));

            // Assert
            int expectedAvailableCopies = 2;
            int actualAvailableCopies = library.GetAvailableCopies("Book 5");
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
        }

        [Test]
        public void ReturnBook_ExistingBookAfterTwoWeeks_ShouldIncreaseAvailableCopiesAndCalculatePenalty()
        {
            // Arrange
            Library library = new Library();
            library.AddBook("Book 6", "ISBN-6", 30.0m, 1);

            // Act
            library.BorrowBook("Book 6", "Adrian Razvan", DateTime.Now);
            library.ReturnBook("Book 6", "Adrian Razvan", DateTime.Now.AddDays(21));

            // Assert
            int expectedAvailableCopies = 1;
            int actualAvailableCopies = library.GetAvailableCopies("Book 6");
            decimal expectedPenalty = 2.1m; // 7 days * (30.0 * 0.01)
            BorrowedBook lastBook = library.borrowedbooks[0];
            Assert.AreEqual(expectedAvailableCopies, actualAvailableCopies);
            Assert.AreEqual(expectedPenalty, lastBook.Penalty);
        }
    }
}