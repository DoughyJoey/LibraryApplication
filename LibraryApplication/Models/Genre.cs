using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LibraryApplication.Models
{
    public class Genre
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [DisplayName("Genre Name")]
        public string Name { get; set; }
    }
}