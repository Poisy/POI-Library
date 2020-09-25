using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace POILibrary
{
    public class Library
    {
        #region Properties and Fields

        private List<Book> books;

        public List<Book> Books { get => new List<Book>(books); }

        #endregion

        #region Events and Event Methods

        public event EventHandler<LibraryEventArgs> BooksChanged;

        private void OnBooksChanged(LibraryEventArgs e)
        {
            BooksChanged?.Invoke(this, e);
        }

        public class LibraryEventArgs : EventArgs
        {
            public List<Book> NewBooks { get; set; }
            public Book NewBookAdded { get; set; }
        }

        #endregion

        #region Constructor

        public Library()
        {
            books = FileManager<List<Book>>.LoadBooks();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searchs for a books with a given title and returns <see cref="List{T}"/>
        /// of type <see cref="Book"/>
        /// </summary>
        public static List<Book> SearchBookOnline(string title)
        {
            title = title.Replace(' ', '+');

            string url = "https://www.googleapis.com/books/v1/volumes?q=" + title;

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);

            JObject json = JObject.Parse(html.Result);

            if (json.ContainsKey("items"))
            {
                List<Book> books = new List<Book>();

                foreach (var book in json["items"])
                {
                    List<string> authors = new List<string>();

                    if (book["volumeInfo"]["authors"] != null)
                    {
                        foreach (var author in book["volumeInfo"]["authors"])
                        {
                            authors.Add(author.Value<string>());
                        }
                    }

                    bool hasThumbnail = book["volumeInfo"]["imageLinks"] != null;

                    books.Add(new Book
                    {
                        Title = (string)book["volumeInfo"]["title"],
                        YearReleased = (string)book["volumeInfo"]["publishedDate"],
                        PagesCont = (string)book["volumeInfo"]["pageCount"],
                        Thumbnail = hasThumbnail ? (string)book["volumeInfo"]["imageLinks"]["thumbnail"] : null,
                        Authors = authors
                    });
                }

                return books;
            }
            else
            {
                return new List<Book>();
            }
        }


        public bool AddBook(Book book)
        {
            if (book.Title != null)
            {
                GiveBookID(book);

                books.Add(book);

                OnBooksChanged(new LibraryEventArgs { NewBookAdded = book, NewBooks = Books });
                return true;
            }
            return false;
        }


        public void RemoveBook(Book book)
        {
            Books.Remove(book);

            OnBooksChanged(new LibraryEventArgs { NewBookAdded = book, NewBooks = Books });
        }


        public void ClearLibrary()
        {
            Books.Clear();
        }


        private void GiveBookID(Book book)
        {
            if (Books.Count > 0)
            {
                book.ID = Books[Books.Count - 1].ID + 1;
            }
            else
            {
                book.ID = 0;
            }
        }


        /// <summary>
        /// Always call this method before closing the App. <br/>
        /// This method saves all books stored in the app to a file.
        /// </summary>
        public void SaveBooks()
        {
            FileManager<List<Book>>.SaveBooks(Books);
        }

        #endregion
    }
}
