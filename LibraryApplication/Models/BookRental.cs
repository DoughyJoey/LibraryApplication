using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryApplication.Models
{
    public class BookRental
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int BookID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ScheduledReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        //allows admin to add an additional charge
        public double? AdditionalCharge { get; set; }

        [Required]
        public double RentalPrice { get; set; }

        [Required]
        public string RentalDuration { get; set; }

        //allows access to the StatusEnum
        [Required]
        public StatusEnum Status { get; set; }

        //enum for the different status of a rental
        public enum StatusEnum
        {
            Requested,
            Approved,
            Rejected,
            Rented,
            Closed
        }
    }
}