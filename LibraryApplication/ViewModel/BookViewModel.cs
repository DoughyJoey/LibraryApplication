using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace LibraryApplication.ViewModel
{
    //BookViewModel combines the Book and Genre properties so they can be accessed together
    public class BookViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        public Book Book { get; set; }
    }
}