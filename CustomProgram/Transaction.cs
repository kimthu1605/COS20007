using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public abstract class Transaction
    {
        public int TransactionID { get; private set; }
        public int UserID { get; private set; }
        public int ItemID { get; private set; }
        public TransactionType Type { get; private set; }

        public Transaction(int userID, int itemID, TransactionType type)
        {
            TransactionID = GenerateTransactionID();
            UserID = userID;
            ItemID = itemID;
            Type = type;
        }

        protected abstract int GenerateTransactionID();
    }

    public class BorrowTransaction : Transaction
    {
        public DateTime DueDate { get; private set; }

        public BorrowTransaction(int userID, int itemID, DateTime dueDate)
            : base(userID, itemID, TransactionType.Borrow)
        {
            DueDate = dueDate;
        }

        protected override int GenerateTransactionID()
        {
            // Implement borrow transaction ID generation logic 
            return 0; // Placeholder for simplicity.
        }
    }

    public class ReturnTransaction : Transaction
    {
        public DateTime ReturnDate { get; private set; }

        public ReturnTransaction(int userID, int itemID, DateTime returnDate)
            : base(userID, itemID, TransactionType.Return)
        {
            ReturnDate = returnDate;
        }

        protected override int GenerateTransactionID()
        {
            // Implement return transaction ID generation logic here.
            return 0; // Placeholder for simplicity.
        }
    }

    public enum TransactionType
    {
        Borrow,
        Return
    }
}
