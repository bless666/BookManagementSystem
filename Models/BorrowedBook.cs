namespace LibraryApplication.Models
{
    public class BorrowedBook : Book
    {
        private DateTime borrowdate;
        private string userName;
        private bool borrowed;
        private decimal penalty;

        public DateTime BorrowDate
        {
            get { return borrowdate; }
            set { borrowdate = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public bool Borrowed
        {
            get { return borrowed; }
            set { borrowed = value; }
        }

        public decimal Penalty
        {
            get { return penalty; }
            set { penalty = value; }
        }

        public BorrowedBook(string title, string isbn, decimal rentalPrice, int availableCopies)
            : base(title, isbn, rentalPrice, availableCopies, availableCopies)
        {
            borrowdate = DateTime.MinValue;
            userName = string.Empty;
            borrowed = false;
            penalty = 0.0m;
        }

        public void AddBorrowedBook(Book item)
        {
            Title = item.Title;
            ISBN = item.ISBN;
            RentalPrice = item.RentalPrice; 
            AvailableCopies = item.AvailableCopies; 
            TotalCopies = item.TotalCopies;
        }
    }
}
