using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryApplication.Models
{
    public class Book
    {
        //Required fields for security and validation
        //Required makes sure field is not null
        [Required]
        public int ID { get; set; }

        //ISBN is the International Standard Book Number
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        //Sets book availability to be between 0 and 1000
        [Range(0, 1000)]
        public int Availability { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
        public DateTime? DateAdded { get; set; }

        [Required]
        public int GenreID { get; set; }

        //Reference to Genre class
        public Genre Genre { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
        public DateTime PublicationDate { get; set; }

        [Required]
        [Display(Name = "Pages")]
        public int Pages { get; set; }

        [Required]
        //Dimensions of book
        [Display(Name = "Product Dimensions")]
        public string ProductDimensions { get; set; }

        [Required]
        public string Publisher { get; set; }
    }
}