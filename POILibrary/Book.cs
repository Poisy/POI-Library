using System;
using System.Collections.Generic;

namespace POILibrary
{
    public class Book
    {
        public string Title { get; set; }
        public string PagesCont { get; set; }
        public List<string> Authors { get; set; }
        public string YearReleased { get; set; }
        public DateTime YearReaded { get; set; }
        public string Thumbnail { get; set; }
    }
}
