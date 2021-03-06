﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace LibraryApplication.Models
{
    public class IndividualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }

        public string UserID { get; set; }
        public int? GenreID { get; set; }
        public int? BookID { get; set; }
        public int? CustomerID { get; set; }
        public int? MembershipID { get; set; }
        public int? BookRentalID { get; set; }


        public string ActionParameter
        {
            get
            {
                var param = new StringBuilder(@"/");

                if(BookID != null && BookID > 0)
                {
                    param.Append(String.Format("{0}", BookID));
                }

                if (GenreID != null && GenreID > 0)
                {
                    param.Append(String.Format("{0}", GenreID));
                }

                if (CustomerID != null && CustomerID > 0)
                {
                    param.Append(String.Format("{0}", CustomerID));
                }

                if (UserID != null && UserID.Trim().Length > 0)
                {
                    param.Append(String.Format("{0}", UserID));
                }

                if (MembershipID != null && MembershipID > 0)
                {
                    param.Append(String.Format("{0}", MembershipID));
                }

                if (BookRentalID != null && BookRentalID > 0)
                {
                    param.Append(String.Format("{0}", BookRentalID));
                }

                return param.ToString();
            }
        }

    }
}