using System;

namespace POILibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Library();

            var books = Library.SearchBookOnline("pod igoto");

            test.AddBook(books[0]);
            test.AddBook(books[1]);

            test.SaveBooks();
        }
    }
}
