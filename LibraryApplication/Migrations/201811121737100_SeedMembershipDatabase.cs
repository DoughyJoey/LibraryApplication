namespace LibraryApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedMembershipDatabase : DbMigration
    {
        public override void Up()
        {
            //inserts membership values into the database
            //values are for pay per rental, member, or superadmin
            Sql("INSERT INTO [dbo].[Memberships](Name,SignUpFee,ChargeRateSixMonth,ChargeRateTwelveMonth) VALUES('Pay Per Rental',0,25,50)");
            Sql("INSERT INTO [dbo].[Memberships](Name,SignUpFee,ChargeRateSixMonth,ChargeRateTwelveMonth) VALUES('Member',100,15,30)");
            //superadmin will not be charged
            Sql("INSERT INTO [dbo].[Memberships](Name,SignUpFee,ChargeRateSixMonth,ChargeRateTwelveMonth) VALUES('SuperAdmin',0,0,0)");
        }
        
        public override void Down()
        {
        }
    }
}
