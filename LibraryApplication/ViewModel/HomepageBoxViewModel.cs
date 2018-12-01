using LibraryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryApplication.ViewModel
{
    public class HomepageBoxViewModel
    {
        public IEnumerable<HomepageModel> Homepages { get; set; }
    }
}