using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomProgram
{
    public abstract class Item
    {
        public int ItemID { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public bool IsAvailable { get; set; }

        public Item(string title, string author)
        {
            ItemID = GenerateItemID();
            Title = title;
            Author = author;
            IsAvailable = true;
        }

        public void BorrowItem()
        {
            IsAvailable = false;
        }

        public void ReturnItem()
        {
            IsAvailable = true;
        }

        protected abstract int GenerateItemID();
    }
}
