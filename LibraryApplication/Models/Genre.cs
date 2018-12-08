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
        //Required fields for security and validation
        //Required makes sure field is not null
        [Required]
        public int ID { get; set; }

        [Required]
        [DisplayName("Genre")]
        public string Name { get; set; }
    }
}