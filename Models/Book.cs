using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryApplication.Models
{
    public class Book
    {
        private string title;
        private string isbn;
        private decimal rentalprice;
        private int availablecopies;
        private int totalcopies;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public decimal RentalPrice
        {
            get { return rentalprice; }
            set { rentalprice = value; }
        }

        public int AvailableCopies
        {
            get { return availablecopies; }
            set { availablecopies = value; }
        }

        public int TotalCopies
        {
            get { return totalcopies; }
            set { totalcopies = value; }
        }

        public Book(string title, string isbn, decimal rentalPrice, int availableCopies)
        {
            this.title = title;
            this.isbn = isbn;
            this.rentalprice = rentalPrice;
            this.availablecopies = availableCopies;
            this.totalcopies = availableCopies;
        }
    }
}
