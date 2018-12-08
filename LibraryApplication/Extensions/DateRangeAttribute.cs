using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryApplication.Extensions
{
    public class DateRangeAttrbute : RangeAttribute
    {
        //returns whether the date is between a certain minimum and todays date time
        public DateRangeAttrbute(string mininumValue) : base(typeof(DateTime), mininumValue, DateTime.Now.ToShortDateString())
        {

        }
    }
}