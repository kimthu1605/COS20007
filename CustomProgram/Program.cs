using System;
using System.Collections.Generic;
using CustomProgram;

namespace CustomProgram
{
    public class Program
    {
        static List<User> users = new List<User>();
        static List<Book> books = new List<Book>();
        static List<Transaction> transactions = new List<Transaction>();
        

        public static void Main(string[] args)
        {
            VipUser vipUser = RegisterVipUser("vipuser1", "password123");
            Book book = new Book("Sample Book", "Sample Author", "1234567890", "Sample Category");
            AddBookContent(book, "This is the content of the book.");
            User adminUser = new User("Admin", "Admin Address");
            adminUser.IsAdmin = true; // Set to true for admin user


            while (true)
            {
                Console.WriteLine("=== Welcome to the Online Library ===");
                Console.WriteLine("1. User Registration");
                Console.WriteLine("2. Search Books");
                Console.WriteLine("3. Borrow a Book");
                Console.WriteLine("4. Return a Book");
                Console.WriteLine("5. View Borrowing History");
                Console.WriteLine("6. Admin Panel (Login required)");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            UserRegistration();
                            break;
                        case 2:
                            SearchBooks();
                            break;
                        case 3:
                            BorrowBook();
                            break;
                        case 4:
                            ReturnBook();
                            break;
                        case 5:
                            ViewBorrowingHistory();
                            break;
                        case 6:
                            if (AdminLogin())
                            {
                                AdminPanel();
                            }
                            break;
                       
                        case 0:
                            Console.WriteLine("Exiting the Online Library. Thank you!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                        
                           


                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid choice.");
                }
            }
        }
        public static VipUser RegisterVipUser(string username, string password)
        {
            return new VipUser { Username = username, Password = password };
        }
        public static void AddBookContent(Book book, string content)
        {
            book.Content = content;
        }
        public static User CreateUser(string userName, string userAddress)
        {
            User user = new User(userName, userAddress);
            users.Add(user);
            return user;
        }
        public static VipUser CreateVipAccount(string userName)
        {
            VipUser vipUser = new VipUser { Username = userName };
            // Additional logic for VIP account creation can be added here.
            return vipUser;
        }

        public static void UserRegistration()
        {
            Console.WriteLine("=== User Registration ===");
            Console.Write("Please enter your information:\nUser Name: ");
            string userName = Console.ReadLine();
            Console.Write("User Address: ");
            string userAddress = Console.ReadLine();

            // Ask the user if they want to create a VIP account

            Console.Write("Do you want to create a VIP account (Yes/No)? ");
            string createVipAccountInput = Console.ReadLine();

            if (createVipAccountInput.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                VipUser vipUser = CreateVipAccount();
                Console.WriteLine("VIP account created successfully!");
                Console.WriteLine($"User registered successfully! Your User ID is: {vipUser.Username}");
                Console.WriteLine("Now you can read book online");

                // Allow VIP user to select a book to read online

                Console.WriteLine("Do you want to read a book online (Yes/No)?");
                string readOnlineInput = Console.ReadLine();
                if (readOnlineInput.Equals("Yes", StringComparison.OrdinalIgnoreCase))
                {
                    // Present a list of available books and allow the VIP user to choose one
                    Console.WriteLine("Available books:");
                    foreach (Book book in books)
                    {
                        if (book.IsAvailable)
                        {
                            Console.WriteLine($"{book.BookID}. {book.Title} by {book.Author}");
                        }
                    }

                    Console.Write("Enter the Book ID of the book you want to read online: ");
                    if (int.TryParse(Console.ReadLine(), out int selectedBookID))
                    {
                        Book selectedBook = books.FirstOrDefault(book => book.BookID == selectedBookID);

                        if (selectedBook != null)
                        {
                            selectedBook.ReadBookOnline(vipUser);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Book ID. Reading canceled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Reading canceled.");
                    }
                }
            }
            else if (createVipAccountInput.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                // Continue with normal user registration
                User user = CreateUser(userName, userAddress);
                Console.WriteLine($"User registered successfully! Your User ID is: {user.UserID}");
            }
            else
            {
                Console.WriteLine("Invalid input. User registration canceled.");
            }
        }
        public static VipUser CreateVipAccount()
        {
            Console.WriteLine("=== VIP Account Registration ===");

            Console.Write("Enter your desired username: ");
            string username = Console.ReadLine();

            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            VipUser vipUser = new VipUser { Username = username, Password = password };

            

            

            return vipUser;
        }
    
        public static void SearchBooks()
        {
            Console.WriteLine("=== Search Books ===");
            Console.Write("Enter keywords to search for a book: ");
            string keywords = Console.ReadLine();

            List<Book> results = books.FindAll(book =>
                book.Title.Contains(keywords, StringComparison.OrdinalIgnoreCase) ||
                book.Author.Contains(keywords, StringComparison.OrdinalIgnoreCase));

            if (results.Count > 0)
            {
                Console.WriteLine("Results:");
                for (int i = 0; i < results.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Title: {results[i].Title}");
                    Console.WriteLine($"   Author: {results[i].Author}");
                    Console.WriteLine($"   Category: {results[i].Category}");
                    Console.WriteLine($"   Available: {(results[i].IsAvailable ? "Yes" : "No")}");
                }
            }
            else
            {
                Console.WriteLine("No results found.");
            }
        }

        public static void BorrowBook()
        {
            Console.WriteLine("=== Borrow a Book ===");
            Console.Write("Enter the Book ID you want to borrow: ");
            int bookID;

            if (int.TryParse(Console.ReadLine(), out bookID))
            {
                Book book = books.Find(b => b.BookID == bookID);

                if (book != null && book.IsAvailable)
                {
                    User user = GetUser();
                    if (user != null)
                    {
                        DateTime dueDate = DateTime.Now.AddDays(14);
                        Transaction transaction = new BorrowTransaction(user.UserID, book.BookID, dueDate);
                        transactions.Add(transaction);
                        book.BorrowBook();

                        Console.WriteLine("Borrowing successful! Due Date: " + dueDate.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    Console.WriteLine("Borrowing failed. Please check Book ID and availability.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Book ID.");
            }
        }

        public static void ReturnBook()
        {
            Console.WriteLine("=== Return a Book ===");
            Console.Write("Enter the Book ID you want to return: ");
            int bookID;

            if (int.TryParse(Console.ReadLine(), out bookID))
            {
                Book book = books.Find(b => b.BookID == bookID);
                if (book != null)
                {
                    User user = GetUser();
                    if (user != null)
                    {
                        Transaction borrowTransaction = transactions.Find(t =>
                            t.UserID == user.UserID && t.ItemID == book.BookID && t.Type == TransactionType.Borrow);

                        if (borrowTransaction != null)
                        {
                            Transaction transaction = new ReturnTransaction(user.UserID, book.BookID, DateTime.Now);
                            transactions.Add(transaction);
                            book.ReturnBook();

                            Console.WriteLine("Return successful! Returned on: " + DateTime.Now.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            Console.WriteLine("Return failed. You did not borrow this book.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Return failed. User not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Return failed. Book not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Book ID.");
            }
        }

        public static void ViewBorrowingHistory()
        {
            User user = GetUser();
            if (user != null)
            {
                Console.WriteLine("=== View Borrowing History ===");
                Console.WriteLine("Borrowing History for User ID: " + user.UserID);

                List<Transaction> userTransactions = transactions.FindAll(t => t.UserID == user.UserID);

                foreach (var transaction in userTransactions)
                {
                    Book book = books.Find(b => b.BookID == transaction.ItemID);
                    if (book != null)
                    {
                        string transactionType = transaction.Type == TransactionType.Borrow ? "Borrow" : "Return";
                        string dateInfo = transaction.Type == TransactionType.Borrow
                            ? "Due Date: " + ((BorrowTransaction)transaction).DueDate.ToString("yyyy-MM-dd")
                            : "Return Date: " + ((ReturnTransaction)transaction).ReturnDate.ToString("yyyy-MM-dd");

                        Console.WriteLine($"Book: {book.Title}");
                        Console.WriteLine($"Transaction Type: {transactionType}");
                        Console.WriteLine(dateInfo);
                    }
                }
            }
        }

        public static bool AdminLogin()
        {
            Console.WriteLine("=== Admin Panel ===");
            Console.Write("Please enter the admin password: ");
            string password = Console.ReadLine();

            // Add your admin login validation logic here.
            // For simplicity, assume any non-empty password is valid.
            return !string.IsNullOrEmpty(password);
        }

        public static void AdminPanel()
        {
            User adminUser = new User("Admin", "Admin Address");
            adminUser.IsAdmin = true;


            while (true)
            {
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Update a Book");
                Console.WriteLine("3. Delete a Book");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice: ");

                int adminChoice;
                if (int.TryParse(Console.ReadLine(), out adminChoice))
                {
                    switch (adminChoice)
                    {
                        case 1:
                            AddBook();
                            break;
                        case 2:
                            Console.Write("Enter the Book ID of the book you want to update: ");
                            if (int.TryParse(Console.ReadLine(), out int bookID))
                            {
                                Book bookToUpdate = books.FirstOrDefault(book => book.BookID == bookID);
                                if (bookToUpdate != null)
                                {
                                    Console.Write("Enter the new content for the book: ");
                                    string newContent = Console.ReadLine();
                                    bookToUpdate.UpdateBookContent(newContent, adminUser);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Book ID.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input.");
                            }
                            break;
                            
                        case 3:
                            Console.Write("Enter the Book ID of the book you want to delete: ");
                            if (int.TryParse(Console.ReadLine(), out int deleteBookID))
                            {
                                Book bookToDelete = books.FirstOrDefault(book => book.BookID == deleteBookID);
                                if (bookToDelete != null)
                                {
                                    // Ask for confirmation before deleting
                                    Console.Write($"Are you sure you want to delete '{bookToDelete.Title}' by {bookToDelete.Author}? (Yes/No): ");
                                    string confirmation = Console.ReadLine().Trim().ToLower();

                                    if (confirmation == "yes")
                                    {
                                        // Remove the book from the books list
                                        books.Remove(bookToDelete);
                                        Console.WriteLine($"Book '{bookToDelete.Title}' deleted successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Deletion canceled.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Book ID.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input.");
                            }
                            break;
                        case 4:
                            Console.WriteLine("Logging out of the Admin Panel...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid choice.");
                }
            }
        }

        public static User GetUser()
        {
            Console.Write("Enter your User ID: ");
            if (int.TryParse(Console.ReadLine(), out int userID))
            {
                User user = users.Find(u => u.UserID == userID);
                if (user != null)
                {
                    return user;
                }
            }
            Console.WriteLine("User not found. Please enter a valid User ID.");
            return null;
        }
        public static void AddBook()
        {
            Console.WriteLine("=== Add a Book ===");

            // Increment the book ID counter
            int nextBookID = GetNextBookID();

            Console.Write("Enter book details:\nISBN: ");
            string isbn = Console.ReadLine();
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Author: ");
            string author = Console.ReadLine();
            Console.Write("Category: ");
            string category = Console.ReadLine();
            Console.Write("Availability (Yes/No): ");
            string availabilityInput = Console.ReadLine().Trim().ToLower();

            if (availabilityInput == "yes" || availabilityInput == "no")
            {
                bool isAvailable = availabilityInput == "yes";

                Book book = new Book(title, author, isbn, category);
                book.IsAvailable = isAvailable;
                book.BookID = nextBookID; 
                books.Add(book);

                Console.WriteLine($"Book added successfully! Book ID: {book.BookID}");
            }
            else
            {
                Console.WriteLine("Invalid availability input. Please enter 'Yes' or 'No'.");
            }
        }

        private static int bookIDCounter = 200; // Initial book ID counter value

        private static int GetNextBookID()
        {
            return ++bookIDCounter;
        }



    }
}



