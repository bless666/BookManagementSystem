using BookManagementSystem;
using BookManagementSystem.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace BookManagementSystemTest
{
    [TestFixture]
    public class BookManagementSystemTest
    {
        private Library library;

        [SetUp]
        public void Setup()
        {
            library = new Library();
        }

        //Unit Tests
        [Test]
        public void AddBook_NewBook_AddsBookToLibrary()
        {
            // Arrange
            string title = "Book Title";
            string isbn = "ISBN123";
            decimal rentalPrice = 9.99m;
            int availableCopies = 5;

            // Act
            library.AddBook(title, isbn, rentalPrice, availableCopies);

            // Assert
            Book addedBook = library.GetBook(title);
            Assert.IsNotNull(addedBook, "Book should be added to the library");
            Assert.AreEqual(title, addedBook.Title, "Book title should match");
            Assert.AreEqual(isbn, addedBook.ISBN, "Book ISBN should match");
            Assert.AreEqual(rentalPrice, addedBook.RentalPrice, "Book rental price should match");
            Assert.AreEqual(availableCopies, addedBook.AvailableCopies, "Book available copies should match");
            Assert.AreEqual(availableCopies, addedBook.TotalCopies, "Book total copies should match");
        }

        [Test]
        public void AddBook_WhenNewBookAdded_ShouldIncreaseAvailableCopies()
        {
            // Arrange
            string title = "Book1";
            string isbn = "ISBN123";
            decimal rentalPrice = 10.99m;
            int availableCopies = 3;

            // Act
            library.AddBook(title, isbn, rentalPrice, availableCopies);

            // Assert
            Assert.That(availableCopies, Is.EqualTo(library.GetAvailableCopies(title)));
        }

        [Test]
        public void AddBook_ExistingBook_ShouldUpdateAvailableCopies()
        {
            // Arrange
            library.AddBook("Book 1", "ISBN-1", 10.0m, 5);

            // Act
            library.AddBook("Book 1", "ISBN-1", 10.0m, 3);

            // Assert
            int expectedAvailableCopies = 8;
            int actualAvailableCopies = library.GetAvailableCopies("Book 1");
            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
        }

        [Test]
        public void AddBook_ExistingBook_ShouldUpdateAvailableCopiesAndISBNorPrice()
        {
            // Arrange
            library.AddBook("Book 1", "ISBN-1", 10.0m, 5);

            // Act
            Console.SetIn(new StringReader("Y"));
            library.AddBook("Book 1", "ISBN-1", 20.0m, 3);

            // Assert
            int expectedAvailableCopies = 8;
            int actualAvailableCopies = library.GetAvailableCopies("Book 1");

            Book expectedbook = new Book("Book 1", "ISBN-1", 20.0m, 8);
            Book actualbook = library.GetBook("Book 1");

            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
            Assert.That(expectedbook.Title, Is.EqualTo(actualbook.Title), "Book title should match");
            Assert.That(expectedbook.ISBN, Is.EqualTo(actualbook.ISBN), "Book ISBN should match");
            Assert.That(expectedbook.RentalPrice, Is.EqualTo(actualbook.RentalPrice), "Book rental price should match");
            Assert.That(expectedbook.AvailableCopies, Is.EqualTo(actualbook.AvailableCopies), "Book available copies should match");
            Assert.That(expectedbook.TotalCopies, Is.EqualTo(actualbook.TotalCopies), "Book total copies should match");
        }

        //Integration Tests

        [Test]
        public void BorrowBook_ExistingBookWithAvailableCopies_ShouldDecreaseAvailableCopies()
        {
            // Arrange
            library.AddBook("Book 3", "ISBN-3", 15.0m, 5);

            // Act
            library.BorrowBook("Book 3", "Adrian Razvan", DateTime.Now);

            // Assert
            int expectedAvailableCopies = 4;
            int actualAvailableCopies = library.GetAvailableCopies("Book 3");
            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
        }

        [Test]
        public void BorrowBook_ExistingBookWithNoAvailableCopies_ShouldNotChangeAvailableCopies()
        {
            // Arrange
            library.AddBook("Book 4", "ISBN-4", 20.0m, 0);

            // Act
            library.BorrowBook("Book 4", "Adrian Razvan", DateTime.Now);

            // Assert
            int expectedAvailableCopies = 0;
            int actualAvailableCopies = library.GetAvailableCopies("Book 4");
            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
        }


        //Functional Tests
        [Test]
        public void ReturnBook_ExistingBookWithinTwoWeeks_ShouldIncreaseAvailableCopiesAndNoPenalty()
        {
            // Arrange
            library.AddBook("Book 5", "ISBN-5", 25.0m, 2);

            // Act
            library.BorrowBook("Book 5", "Adrian Razvan", DateTime.Now);
            library.ReturnBook("Book 5", "Adrian Razvan", DateTime.Now.AddDays(10));

            // Assert
            int expectedAvailableCopies = 2;
            int actualAvailableCopies = library.GetAvailableCopies("Book 5");
            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
        }

        [Test]
        public void ReturnBook_ExistingBookAfterTwoWeeks_ShouldIncreaseAvailableCopiesAndCalculatePenalty()
        {
            // Arrange
            library.AddBook("Book 6", "ISBN-6", 30.0m, 1);

            // Act
            library.BorrowBook("Book 6", "Adrian Razvan", DateTime.Now);
            library.ReturnBook("Book 6", "Adrian Razvan", DateTime.Now.AddDays(21));

            // Assert
            int expectedAvailableCopies = 1;
            int actualAvailableCopies = library.GetAvailableCopies("Book 6");
            decimal expectedPenalty = 2.1m; // 7 days * (30.0 * 0.01)
            BorrowedBook lastBook = library.borrowedbooks[0];

            Assert.That(expectedAvailableCopies, Is.EqualTo(actualAvailableCopies));
            Assert.That(expectedPenalty, Is.EqualTo(lastBook.Penalty));
        }
    }
}