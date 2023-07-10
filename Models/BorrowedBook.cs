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

        public BorrowedBook(string title, string isbn, decimal rentalPrice, int availableCopies, 
            DateTime borrowdate, string userName, bool borrowed, decimal penalty)
            : base(title, isbn, rentalPrice, availableCopies)
        {
            this.borrowdate = borrowdate;
            this.userName = userName;
            this.borrowed = borrowed;
            this.penalty = penalty;
        }
    }
}
