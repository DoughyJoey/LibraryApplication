using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LibraryApplication.Models
{
    public class Membership
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public byte SignUpFee { get; set; }

        [DisplayName("Rental Rate")]
        [Required]
        public byte ChargeRateSixMonth { get; set; }

        [Required]
        public byte ChargeRateTwelveMonth { get; set; }
    }
}