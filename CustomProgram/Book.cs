using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public class Book : Item
    {
        public int BookID { get; set; } // Added BookID property
        public string ISBN { get; private set; }
        public string Category { get; private set; }
        public string Content { get; set; }



        public Book(string title, string author, string isbn, string category) : base(title, author)
        {
            BookID = GenerateItemID(); // Set the BookID
            ISBN = isbn;
            Category = category;
            Content = string.Empty;
        }
        // VIP registration function
        public static VipUser RegisterVipUser(string username, string password)
        {
            return new VipUser { Username = username, Password = password };
        }
        // Read book online function
        public void ReadBookOnline(VipUser vipUser)
        {
            if (IsAvailable && !string.IsNullOrEmpty(Content) && vipUser != null)
            {
                Console.WriteLine($"VIP user '{vipUser.Username}' is reading '{Title}' by {Author} (Category: {Category}):");
                Console.WriteLine(Content);
            }
            else
            {
                Console.WriteLine("The book is not available for online reading by the VIP user.");
            }
        }
        protected override int GenerateItemID()
        {
            // Implement book-specific ID 
            return 0; // Placeholder for simplicity.
        }

        public void BorrowBook()
        {
            IsAvailable = false;
        }

        public void ReturnBook()
        {
            IsAvailable = true;
        }
        public void UpdateBookContent(string newContent, User user)
        {
            if (user.IsAdmin)
            {
                Content = newContent;
                Console.WriteLine("Book content updated successfully.");
            }
            else
            {
                Console.WriteLine("You do not have permission to update book content.");
            }
        }


    }
}
