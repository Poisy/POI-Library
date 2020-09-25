using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace POILibrary
{
    public class LibraryManager
    {
        private readonly static string googleBookAPIUrl = "https://www.googleapis.com/books/v1/volumes?q=";

        /// <summary>
        /// Searchs for a books with a given title and returns <see cref="List{T}"/>
        /// of type <see cref="Book"/>
        /// </summary>
        public static List<Book> ScrapBooks(string title)
        {
            title = title.Replace(' ', '+');

            string url = googleBookAPIUrl + title;

            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);

            JObject json = JObject.Parse(html.Result);

            if (json.ContainsKey("items"))
            {
                List<Book> books = new List<Book>();

                foreach (var book in json["items"])
                {
                    List<string> authors = new List<string>();

                    foreach (var author in book["volumeInfo"]["authors"])
                    {
                        authors.Add(author.Value<string>());
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



    }
}
