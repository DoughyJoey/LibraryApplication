using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LibraryApplication.Models;
using System.ComponentModel;

namespace LibraryApplication.ViewModel
{
    public class UserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public ICollection<Membership> Memberships { get; set; }

        [DisplayName("Membership Type")]
        [Required]
        public int MembershipID { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [DisplayName("Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM dd yyyy}")]
        public DateTime BirthDate { get; set; }

        public bool Disabled { get; set; }


    }
}