namespace LibraryApplication.Helper
{
    public class Helper
    {
        //Parse the insert into a date with this specific format "dd/MM/yyyy"
        public static DateTime ParseDate(string prompt)
        {
            string dateString;
            DateTime parsedDate = DateTime.MinValue;
            do
            {
                Console.Write(prompt);
                dateString = Console.ReadLine();
                if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    Console.WriteLine("The inserted date is not in the specified format.");
                }
            } while (parsedDate == DateTime.MinValue);
            return parsedDate;
        }

        //Check if the insert is empty 
        public static string ReadStringValue(string prompt)
        {
            string value;
            do
            {
                Console.Write(prompt);
                value = Console.ReadLine();
                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine("Invalid input. Please enter a non-empty value.");
                }
            } while (string.IsNullOrEmpty(value));
            return value;
        }

        //Check if the insert is Y or N for questions answer
        public static string ReadStringQuestionResponse(string prompt)
        {
            string value;
            do
            {
                Console.Write(prompt);
                value = Console.ReadLine()?.ToUpperInvariant(); // Convert input to uppercase for case-insensitive comparison
                if (value != "Y" && value != "N")
                {
                    Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
                }
            } while (value != "Y" && value != "N");
            return value;
        }

        //Check if the inserted value is a int
        public static int ReadIntValue(string prompt)
        {
            int value;
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!int.TryParse(input, out value) || value < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a non-negative integer value.");
                }
            } while (!int.TryParse(input, out value) || value < 0);
            return value;
        }

        //Check if the inserted value is a decimal
        public static decimal ReadDecimalValue(string prompt)
        {
            decimal value;
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!decimal.TryParse(input, out value) || value <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive decimal value.");
                }
            } while (!decimal.TryParse(input, out value) || value <= 0);
            return value;
        }
    }
}
