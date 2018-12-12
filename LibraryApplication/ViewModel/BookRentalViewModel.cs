using LibraryApplication.Models;
using LibraryApplication.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryApplication.ViewModel
{
    public class BookRentalViewModel
    {
        public int ID { get; set; }

        //BOOK INFORMATION
        public int BookID { get; set; }
        [DisplayName("Serial Number")]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }


        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Range(0, 1000)]
        [DisplayName("Availability")]
        public int Availability { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? DateAdded { get; set; }

        public int GenreID { get; set; }

        public Genre Genre { get; set; }

        public string Publisher { get; set; }

        [DisplayName("Publication Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime PublicationDate { get; set; }

        [DisplayName("Pages")]
        public int Pages { get; set; }

        [DisplayName("Product Dimensions")]
        public string ProductDimensions { get; set; }

        //RENTAL INFORMATION
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Scheduled Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? ScheduledReturnDate { get; set; }

        [DisplayName("Actual Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? ActualReturnDate { get; set; }

        [DisplayName("Additional Charge")]
        public double? AdditionalCharge { get; set; }

        [DisplayName("Rental Price")]
        public double RentalPrice { get; set; }

        [DisplayName("Duration")]
        public string RentalDuration { get; set; }

        public String Status { get; set; }

        [DisplayName("6 Month Price")]
        public double RentalPriceSixMonth { get; set; }

        [DisplayName("12 Month Price")]
        public double RentalPriceTwelveMonth { get; set; }

        //USER INFORMATION
        public string UserID { get; set; }
        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Name { get { return FirstName + " " + LastName; } }

        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
        public DateTime? BirthDate { get; set; }

        public string actionName
        {
            get
            {
                //if requested show approved
                if (Status.ToLower().Contains(StaticDetails.RequestedLower))
                {
                    return "Approve";
                }

                //if approved show pickup
                if (Status.ToLower().Contains(StaticDetails.ApprovedLower))
                {
                    return "PickUp";
                }

                //if rented show return
                if (Status.ToLower().Contains(StaticDetails.RentedLower))
                {
                    return "Return";
                }
                return null;
            }
        }
    }
}