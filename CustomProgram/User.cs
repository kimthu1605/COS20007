using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public class User
    {
        public int UserID { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public bool IsAdmin { get; set; } 

        public User(string name, string address)
        {
            UserID = GenerateUserID();
            Name = name;
            Address = address;
            IsAdmin = false; // Initialize IsAdmin as false by default

        }

        private int GenerateUserID()
        {
            // Implement user ID generation logic here.
            return 0; // Placeholder for simplicity.
        }
    }
}
