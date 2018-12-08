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

        //Name refers to the name of the type of membership
        [Required]
        [DisplayName("Membership Type")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Sign Up Fee")]
        [DataType(DataType.Currency)]
        public byte SignUpFee { get; set; }

        [DisplayName("6 Month Rate")]
        [Required]
        public byte ChargeRateSixMonth { get; set; }

        [DisplayName("12 Month Rate")]
        [Required]
        public byte ChargeRateTwelveMonth { get; set; }
    }
}