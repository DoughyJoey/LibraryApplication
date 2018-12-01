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

        public double? AdditionalCharge { get; set; }

        [Required]
        public double RentalPrice { get; set; }

        [Required]
        public string RentalDuration { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

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