using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryApplication.Models;

namespace LibraryApplication.ViewModel
{
    public class BookViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        public Book Book { get; set; }
    }
}