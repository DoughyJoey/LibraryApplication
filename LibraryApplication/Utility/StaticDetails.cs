using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LibraryApplication.Utility
{
    public class StaticDetails
    {
        //DIFFERENT USER ROLES
        public const string EndUserRole = "Customer";
        public const string AdminUserRole = "SuperAdmin";

        //DIFFERENT RENTAL DURATIONS
        public const string SixMonth = "Six Month";
        public const string TwelveMonth = "Twelve Month";

        public const string SixMonthCount = "6";
        public const string TwelveMonthCount = "12";

        //DIFFERENT BOOK RENTAL STATES
        public const string RequestedLower = "requested";
        public const string ApprovedLower = "approved";
        public const string RentedLower = "rented";
        public const string PickUpLower = "pickup";
        public const string ClosedLower = "closed";
    }
}